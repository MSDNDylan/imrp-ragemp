const NativeUI = require("./IMRP/nativeui/index");
const Menu = NativeUI.Menu;
const UIMenuItem = NativeUI.UIMenuItem;
const UIMenuListItem = NativeUI.UIMenuListItem;
const UIMenuCheckboxItem = NativeUI.UIMenuCheckboxItem;
const UIMenuSliderItem = NativeUI.UIMenuSliderItem;
const BadgeStyle = NativeUI.BadgeStyle;
const Point = NativeUI.Point;
const ItemsCollection = NativeUI.ItemsCollection;
const Color = NativeUI.Color;
const ListItem = NativeUI.ListItem;

mp.gui.cursor.visible = false;
mp.gui.chat.show(true);

var ui = undefined;
var activeMenuID = "";

mp.events.add(
    {
        "buildMenu": (menuData) =>{
			
			if(ui !== undefined)
			{
				activeMenuID = "";
				ui.visible = false;
				ui.Close();
				ui = undefined;
			}

			menuData = JSON.parse(menuData);
			if(menuData.MenuItems.length < 0) return;
			activeMenuID = menuData.MenuID;
			ui = new Menu(menuData.Title, menuData.SubTitle, new Point(menuData.Point.X,menuData.Point.Y));

			menuData.MenuItems.forEach(e => {
				switch(e.MenuType){
					case "UIMenuCheckboxItem":
						ui.AddItem(new UIMenuCheckboxItem(
							e.Properties.Caption,
							e.Properties.Checked,
							e.Properties.Description
						));						
						break;
					case "UIMenuListItem":
						ui.AddItem(new UIMenuListItem(
							e.Properties.Caption,
							e.Properties.Description,
							new ItemsCollection(e.Properties.Fields)
						));
					break;
					case "UIMenuSliderItem":
						ui.AddItem(new UIMenuSliderItem(
							e.Properties.Caption,
							e.Properties.Fields,
							e.Properties.StartIndex,
							e.Properties.Description,
							true
						));
						break;
					case "UIMenuItem":

						var newMenu = new UIMenuItem(
							e.Properties.Caption,
							e.Properties.Description
						);
						if(e.Properties.BackColor != null)
						{
							newMenu.BackColor = new Color(e.Properties.BackColor.Red, e.Properties.BackColor.Green, e.Properties.BackColor.Blue, e.Properties.BackColor.Alpha);
						}
						if(e.Properties.HighlightedBackColor != null)
						{
							newMenu.HighlightedBackColor = new Color(e.Properties.HighlightedBackColor.Red, e.Properties.HighlightedBackColor.Green, e.Properties.HighlightedBackColor.Blue, e.Properties.HighlightedBackColor.Alpha);
						}
						if(e.Properties.ForeColor != null)
						{
							newMenu.ForeColor = new Color(e.Properties.ForeColor.Red, e.Properties.ForeColor.Green, e.Properties.ForeColor.Blue, e.Properties.ForeColor.Alpha);
						}
						if(e.Properties.HighlightedForeColor != null)
						{
							newMenu.HighlightedForeColor = new Color(e.Properties.ForeColor.Red, e.Properties.ForeColor.Green, e.Properties.ForeColor.Blue, e.Properties.ForeColor.Alpha);
						}

						ui.AddItem(newMenu);
						break;
				}
			});
			ui.Open();
			ui.visible = true;
			startListening();
		},
		'destroyMenu':() =>{
			if(ui !== undefined)
			{
				activeMenuID = "";
				ui.visible = false;
				ui.Close();
				ui = undefined;
			}
		}
    }
)

function startListening()
{
	ui.ItemSelect.on(item => {
		if (item instanceof UIMenuListItem) {
			//mp.events.callRemote('InvokeNativeUIListener', "UIMenuListItem",item.SelectedItem.DisplayText,item.SelectedItem.Data);
			mp.events.callRemote('InvokeNativeUIListener', "UIMenuListItem",activeMenuID,item.SelectedItem.DisplayText,item.SelectedItem.Data);
		} else if (item instanceof UIMenuSliderItem) {
			//mp.events.callRemote('InvokeNativeUIListener', "UIMenuSliderItem",item.Text, item.Index, item.IndexToItem(item.Index));
			mp.events.callRemote('InvokeNativeUIListener', "UIMenuSliderItem",activeMenuID,item.Text, item.Index, item.IndexToItem(item.Index));
		} else if(item instanceof UIMenuItem){
			mp.events.callRemote('InvokeNativeUIListener', "UIMenuItem",activeMenuID,item.Text);
		}
		else
		{
			mp.events.callRemote('InvokeNativeUIListener', "Unknown",activeMenuID,item.Text);
		}
	});
}