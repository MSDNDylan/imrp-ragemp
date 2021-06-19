hideForm(`#confirmFinished`);
hideForm(`#confirmCancel`);

hideForm('#browOptions');
hideForm('#cheekBoneOptions');
hideForm('#eyesOptions');
hideForm('#lipsOptions');
hideForm('#jawOptions');
hideForm('#chinOptions');
hideForm('#neckOptions');

hideForm('#makeupForm');
hideForm('#faceForm');
hideForm('#headForm');
hideForm('#faceDetailForm');
hideForm('#bodyForm');
showForm(`#genderForm`);


var activeOption = "#genderOption";
var activeForm = "#genderForm";
$(activeOption).addClass('Active');

var activeFaceOption = "#noseOption";
var activeFaceForm = "#noseOptions";
$(activeFaceOption).addClass('Active');

let charData = {
    'Head' : [
        {index : 255, opacity: 1.0, color: 0},
        {index : 255, opacity: 1.0, color: 0},
        {index : 255, opacity: 1.0, color: 0},
        {index : 255, opacity: 1.0, color: 0},
        {index : 255, opacity: 1.0, color: 0},
        {index : 255, opacity: 1.0, color: 0},
        {index : 255, opacity: 1.0, color: 0},
        {index : 255, opacity: 1.0, color: 0},
        {index : 255, opacity: 1.0, color: 0},
        {index : 255, opacity: 1.0, color: 0},
        {index : 255, opacity: 1.0, color: 0},
        {index : 255, opacity: 1.0, color: 0}
    ],
    'Gender': 0,
    'Hair' : { style: 0, color: 0, highlights: 0 },
    'SkinColor': 0,
    'EyeColor': 0,
    'Face' : { 
        mother: 0, 
        father: 0, 
        character: 0,
        resemblance: 0.0,
        data : [
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
        ]
    },
    'Clothes' : {
        top: { ctype : 'T-Shirt', cid : 0 },
        pants: { ctype : 'Jean', cid : 0 },
        shoes:  { ctype : 'Sneaker', cid : 0 }
    }
};

$('#noseOption').click(() => {
    toggleFaceOption(`nose`);
});
$('#browOption').click(() => {
    toggleFaceOption(`brow`);
});
$('#cheekBoneOption').click(() => {
    toggleFaceOption(`cheekBone`);
});
$('#eyesOption').click(() => {
    toggleFaceOption(`eyes`);
});
$('#lipsOption').click(() => {
    toggleFaceOption(`lips`);
});
$('#jawOption').click(() => {
    toggleFaceOption(`jaw`);
});
$('#chinOption').click(() => {
    toggleFaceOption(`chin`);
});
$('#neckOption').click(() => {
    toggleFaceOption(`neck`);
});

$('#genderOption').click(() => {
    toggleOption(`gender`);
    mp.trigger('creator_FaceCam', false);
});

$('#headOption').click(() => {
    toggleOption(`head`);
    mp.trigger('creator_FaceCam', true);
});

$('#faceOption').click(() => {
    toggleOption(`face`);
    mp.trigger('creator_FaceCam', true);
});

$('#faceDetailOption').click(() => {
    toggleOption(`faceDetail`);
    mp.trigger('creator_FaceCam', true);
});
$('#bodyOption').click(() => {
    toggleOption(`body`);
    mp.trigger('creator_FaceCam', false);
});

$('#makeupOption').click(() => {
    toggleOption(`makeup`);
    mp.trigger('creator_FaceCam', true);
});

$('#maleButton').click(() => {
    $('#maleButton').addClass('Active');
    $('#femaleButton').removeClass('Active');
    charData.Gender = 0;
    mp.trigger('creator_ChangeGender', charData.Gender);
});

$('#femaleButton').click(() => {
    $('#maleButton').removeClass('Active');
    $('#femaleButton').addClass('Active');
    charData.Gender = 1;
    mp.trigger('creator_ChangeGender', charData.Gender);
});

$('#promptFinish').click(() => {
    $('#creatorError').text("");

    var firstname = $('#characterFirstname').val();
    var lastname = $('#characterLastname').val();
    var age = $('#characterAge').val();

    if(firstname == ''){
        $('#creatorError').text("A firstname is required.");
        return;
    }
    else if(lastname == ''){
        $('#creatorError').text("A lastname is required.");
        return;
    }
    else if(lastname.length < 4){
        $('#creatorError').text("Lastname must at least be 4 characters.");
        return;
    }
    else if(firstname.length < 4){
        $('#creatorError').text("Firstname must at least be 4 characters.");
        return;
    }
    else if(/^[a-zA-Z0-9- ]*$/.test(firstname) == false) {
        $('#creatorError').text("Firstname must not contain special characters.");
        return;
    }
    else if(/^[a-zA-Z0-9- ]*$/.test(lastname) == false) {
        $('#creatorError').text("Lastname must not contain special characters.");
        return;
    }
    else if(hasNumber(firstname)){
        $('#creatorError').text("Firstname must not contain numbers.");
        return;
    }
    else if(hasNumber(lastname)){
        $('#creatorError').text("Lastname must not contain numbers.");
        return;
    }
    else if(parseInt(age) < 18 || age == ''){
        $('#creatorError').text("Character must be 18 or older.");
        return;    
    }
    firstname = firstname.charAt(0).toUpperCase() + firstname.substr(1).toLowerCase();
    lastname = lastname.charAt(0).toUpperCase() + lastname.substr(1).toLowerCase();

    hideForm(`#roleplayInfo`);
    hideForm(`#characterOptions`);
    hideForm(activeForm);
    $('#finishLabel').html(`${firstname} ${lastname}<br>Age: ${age}`);
    showForm(`#confirmFinished`);
});

function hasNumber(myString) {
    return /\d/.test(myString);
  }

$('#cancelFinish').click(() => {
    hideForm(`#confirmFinished`);

    showForm(`#roleplayInfo`);
    showForm(`#characterOptions`);
    showForm(activeForm);
});

$('#finishCharacter').click(() => {
    hideForm(`#confirmFinished`);

    var firstname = $('#characterFirstname').val();
    var lastname = $('#characterLastname').val();
    var age = $('#characterAge').val();
    
    firstname = firstname.charAt(0).toUpperCase() + firstname.substr(1).toLowerCase();
    lastname = lastname.charAt(0).toUpperCase() + lastname.substr(1).toLowerCase();
    
    mp.trigger('creator_Finish', firstname, lastname, age);
});

$('#promptCancel').click(() => {
    showForm(`#confirmCancel`);
});

$('#cancelCancel').click(() => {
    hideForm(`#confirmCancel`);
});

$('#cancelCreator').click(() => {
    mp.trigger('transitionToSelector');
});
$('#finishCharacter1').click(() => {
    var firstname = $('#characterFirstname').val();
    var lastname = $('#characterLastname').val();
    var age = $('#characterAge').val();

    if(firstname.length() <= 0)
    {
        $('#creatorError').text("A firstname is required.");
        return;
    }
    if(lastname.length() <= 0)
    {
        $('#creatorError').text("A lastname is required.");
        return;
    }
    if(firstmane.length() > 20)
    {
        $('#creatorError').text("Firstname is too large. Must be less than 20 characters.");
        return;
    }
    if(lastname.length() > 20)
    {
        $('#creatorError').text("Lastname is too large. Must be less than 20 characters.");
        return;
    }

    if(/^[a-zA-Z0-9- ]*$/.test(firstname) == false) {
        $('#creatorError').text("Firstname must not contain special characters.");
        return;
    }

    if(/^[a-zA-Z0-9- ]*$/.test(lastname) == false) {
        $('#creatorError').text("Lastname must not contain special characters.");
        return;
    }

    firstname = firstname.charAt(0).toUpperCase() + firstname.substr(1).toLowerCase();
    lastname = lastname.charAt(0).toUpperCase() + lastname.substr(1).toLowerCase();

    mp.trigger('creator_Finish', firstname, lastname, age);
    mp.trigger('transitionToSelector');
});


$('#fatherSlider').on('input', 
    function(ev){ 
        charData.Face.father = parseInt($('#fatherSlider').val());
        mp.trigger('creator_changeParents', charData.Face.father, charData.Face.mother, charData.Face.resemblance);
});

$('#motherSlider').on('input', 
    function(ev){ 
        charData.Face.mother = parseInt($('#motherSlider').val());
        mp.trigger('creator_changeParents', charData.Face.father, charData.Face.mother, charData.Face.resemblance);
});

$('#resemblanceSlider').on('input', 
    function(ev){ 
        charData.Face.resemblance = parseFloat($('#resemblanceSlider').val());
        mp.trigger('creator_changeParents', charData.Face.father, charData.Face.mother, charData.Face.resemblance);
});

$('#hairStyleSlider').on('input', 
    function(ev){
        charData.Hair.style = parseInt($('#hairStyleSlider').val());
        mp.trigger('creator_changeHair', charData.Hair.style, charData.Hair.color, charData.Hair.highlights);
});

$('#hairColorSlider').on('input', 
    function(ev){ 
        charData.Hair.color = parseInt($('#hairColorSlider').val());
        mp.trigger('creator_changeHair', charData.Hair.style, charData.Hair.color, charData.Hair.highlights);
});

$('#hairHighlightSlider').on('input', 
    function(ev){ 
        charData.Hair.highlights = parseInt($('#hairHighlightSlider').val());
        mp.trigger('creator_changeHair', charData.Hair.style, charData.Hair.color, charData.Hair.highlights);
});

$('#eyebrowSlider').on('input', 
    function(ev){ 
        charData.Head[2].index = (parseInt($('#eyebrowSlider').val()) == 0) ? 255 : parseInt($('#eyebrowSlider').val()) - 1;
       mp.trigger('creator_setHeadOverlay', 2, charData.Head[2].index, charData.Head[2].opacity);
});

$('#eyebrowThicknessSlider').on('input', 
    function(ev){ 
       charData.Head[2].opacity = parseFloat($('#eyebrowThicknessSlider').val());
       mp.trigger('creator_setHeadOverlay', 2, charData.Head[2].index, charData.Head[2].opacity);
});

$('#eyebrowColorSlider').on('input', 
    function(ev){ 
        charData.Head[1].color = parseInt($('#eyebrowColorSlider').val());
        mp.trigger('creator_setHeadOverlayColor', 2, charData.Head[1].color);
});

$('#beardSlider').on('input', 
    function(ev){ 
        charData.Head[1].index = (parseInt($('#beardSlider').val()) == 0) ? 255 : parseInt($('#beardSlider').val()) - 1;
        mp.trigger('creator_setHeadOverlay', 1, charData.Head[1].index, charData.Head[1].opacity);
});

$('#beardThicknessSlider').on('input', 
    function(ev){ 
        charData.Head[1].opacity = parseFloat($('#beardThicknessSlider').val());
        mp.trigger('creator_setHeadOverlay', 1, charData.Head[1].index, charData.Head[1].opacity);
});

$('#beardColorSlider').on('input', 
    function(ev){ 
       charData.Head[1].color = parseInt($('#beardColorSlider').val());
       mp.trigger("creator_setHeadOverlayColor", 1, charData.Head[1].color);
});

$('#skinBlemishSlider').on('input', 
    function(ev){ 
       charData.Head[0].opacity = parseInt($('#skinBlemishSlider').val());
       mp.trigger('creator_setHeadOverlay', 0, charData.Head[0].index, charData.Head[0].opacity);
});

$('#skinAgingSlider').on('input', 
    function(ev){ 
        charData.Head[3].index = (parseInt($('#skinAgingSlider').val()) == 0) ? 255 : parseInt($('#skinAgingSlider').val())- 1;
        mp.trigger('creator_setHeadOverlay', 3, charData.Head[3].index, charData.Head[3].opacity);
});

$('#agingOpacitySlider').on('input', 
    function(ev){ 
        charData.Head[3].opacity = parseFloat($('#agingOpacitySlider').val());
        mp.trigger('creator_setHeadOverlay', 3, charData.Head[3].index, charData.Head[3].opacity);
});

$('#skinComplexionSlider').on('input', 
    function(ev){ 
        charData.Head[6].index = (parseInt($('#skinComplexionSlider').val()) == 0) ? 255 : parseInt($('#skinComplexionSlider').val()) - 1;
        mp.trigger('creator_setHeadOverlay', 6, charData.Head[6].index, charData.Head[6].opacity);
});

$('#complexionOpacitySlider').on('input', 
    function(ev){ 
        charData.Head[6].opacity = parseFloat($('#complexionOpacitySlider').val());
        mp.trigger('creator_setHeadOverlay', 6, charData.Head[6].index, charData.Head[6].opacity);
});

$('#molesFrecklesSlider').on('input', 
    function(ev){ 
        charData.Head[7].index = (parseInt($('#molesFrecklesSlide').val()) == 0) ? 255 : parseInt($('#molesFrecklesSlide').val()) - 1;
        mp.trigger('creator_setHeadOverlay', 7, charData.Head[7].index, charData.Head[7].opacity);
});

$('#moleFreckleOpacitySlider').on('input', 
    function(ev){ 
        charData.Head[9].opacity = parseFloat($('#moleFreckleOpacitySlider').val());
        mp.trigger('creator_setHeadOverlay', 9, charData.Head[9].index, charData.Head[9].opacity);
});

$('#skinDamageSlider').on('input', 
    function(ev){ 
        charData.Head[7].index = (parseInt($('#skinDamageSlider').val()) == 0) ? 255 : parseInt($('#skinDamageSlider').val()) - 1;
        mp.trigger('creator_setHeadOverlay', 7, charData.Head[7].index, charData.Head[7].opacity);
});

$('#damageOpacitySlider').on('input', 
    function(ev){ 
        charData.Head[7].opacity = parseFloat($('#damageOpacitySlider').val());
        mp.trigger('creator_setHeadOverlay', 7, charData.Head[7].index, charData.Head[7].opacity);
});

$('#makeupSlider').on('input', 
    function(ev){ 
        charData.Head[10].index = (parseInt($('#makeupSlider').val()) == 0) ? 255 : parseInt($('#makeupSlider').val()) - 1;
        mp.trigger('creator_setHeadOverlay', 4, charData.Head[10].index, charData.Head[10].opacity);
});

$('#blushSlider').on('input', 
    function(ev){ 
        charData.Head[5].index = (parseInt($('#blushSlider').val()) == 0) ? 255 : cparseInt($('#blushSlider').val()) - 1;
        mp.trigger('creator_setHeadOverlay', 5, charData.Head[5].index, charData.Head[5].opacity);
});

$('#blushOpacitySlider').on('input', 
    function(ev){ 
        charData.Head[5].opacity = parseFloat($('#blushOpacitySlide').val());
        mp.trigger('creator_setHeadOverlay', 5, charData.Head[5].index, charData.Head[5].opacity);
});

$('#blushColorSlider').on('input', 
    function(ev){ 
        charData.Head[5].color = parseFloat($('#blushColorSlider').val());
        mp.trigger('creator_setHeadOverlayColor', 5, parseFloat($('#blushColorSlider').val()));
});

$('#lipstickSlider').on('input', 
    function(ev){ 
        charData.Head[8].index = (parseInt($('#lipstickSlider').val())== 0) ? 255 : parseInt($('#lipstickSlider').val()) - 1;
        mp.trigger('creator_setHeadOverlay', 8, charData.Head[8].index, charData.Head[8].opacity); 
});

$('#lipstickOpacitySlider').on('input', 
    function(ev){ 
        charData.Head[8].opacity = parseFloat($('#lipstickOpacitySlider').val());
        mp.trigger('creator_setHeadOverlay', 8, charData.Head[8].index, charData.Head[8].opacity);
});

$('#lipstickColorSlider').on('input', 
    function(ev){ 
        charData.Head[8].color = parseInt($('#lipstickColorSlider').val());
        mp.trigger('creator_setHeadOverlayColor', 8, charData.Head[8].color);
});

$('#skinColorSlider').on('input', 
    function(ev){ 
        charData.SkinColor = parseInt($('#skinColorSlider').val());
        mp.trigger('creator_changeSkinColor', charData.SkinColor);
});

$('#bodyBlemishSlider').on('input', 
    function(ev){ 
        charData.Head[11].index = (parseInt($('#bodyBlemishSlider').val()) == 0) ? 255 : parseInt($('#bodyBlemishSlider').val()) - 1;
        mp.trigger('creator_setHeadOverlay', 11, charData.Head[11].index, charData.Head[11].opacity);
});

$('#blemishOpacitySlider').on('input', 
    function(ev){ 
        charData.Head[11].opacity = parseFloat($('#blemishOpacitySlider').val());
        mp.trigger('creator_setHeadOverlay', 11, charData.Head[11].index, charData.Head[11].opacity);
});

$('#chestHairSlider').on('input', 
    function(ev){ 
        charData.Head[10].index = (parseInt($('#chestHairSlider').val()) == 0) ? 255 : parseInt($('#chestHairSlider').val()) - 1;
        mp.trigger('creator_setHeadOverlay', 10, charData.Head[10].index, charData.Head[10].opacity);
});

$('#chestHairOpacity').on('input', 
    function(ev){ 
        charData.Head[10].opacity = parseFloat($('#chestHairOpacity').val());
        mp.trigger('createCharacter_setHeadOverlay', 10, charData.Head[10].index, charData.Head[10].opacity);
});

$('#noseWidthSlider').on('input', 
    function(ev){ 
    charData.Face.data[0] = parseFloat($('#noseWidthSlider').val());
    mp.trigger('creator_setFaceFeature', 0, charData.Face.data[0]);
});

$('#noseHeighSlider').on('input', 
    function(ev){ 
        charData.Face.data[1] = parseFloat($('#noseHeighSlider').val());
        mp.trigger('creator_setFaceFeature', 1, charData.Face.data[1]);
});
$('#noseLengthSlider').on('input', 
    function(ev){ 
        charData.Face.data[2] = parseFloat($('#noseLengthSlider').val());
        mp.trigger('creator_setFaceFeature', 2, charData.Face.data[2]);
});
$('#noseBridgeSlider').on('input', 
    function(ev){ 
        charData.Face.data[3] = parseFloat($('#noseBridgeSlider').val());
        mp.trigger('creator_setFaceFeature', 3, charData.Face.data[3]);
});
$('#noseBridgeShiftSlider').on('input', 
    function(ev){ 
        charData.Face.data[5] = parseFloat($('#noseBridgeShiftSlider').val());
        mp.trigger('creator_setFaceFeature', 5, charData.Face.data[5]);
});
$('#noseTipSlider').on('input', 
    function(ev){ 
        charData.Face.data[4] = parseFloat($('#noseTipSlider').val());
        mp.trigger('creator_setFaceFeature', 4, charData.Face.data[4]);
});
$('#browWidthSlider').on('input', 
    function(ev){ 
        charData.Face.data[7] = parseFloat($('#browWidthSlider').val());
        mp.trigger('creator_setFaceFeature', 7, charData.Face.data[7]);
});
$('#browHeightSlider').on('input', 
    function(ev){ 
        charData.Face.data[6] = parseFloat($('#browHeightSlider').val());
        mp.trigger('creator_setFaceFeature', 6, charData.Face.data[6]);
});
$('#cheekBoneWidthSlider').on('input', 
    function(ev){ 
        charData.Face.data[9] = parseFloat($('#cheekBoneWidthSlider').val());
        mp.trigger('creator_setFaceFeature', 9, charData.Face.data[9]);
});
$('#cheekBoneHeightSlider').on('input', 
    function(ev){ 
        charData.Face.data[8] = parseFloat($('#cheekBoneHeightSlider').val());
        mp.trigger('creator_setFaceFeature', 8, charData.Face.data[8]);
});
$('#cheekWidthSlider').on('input', 
    function(ev){ 
        charData.Face.data[10] = parseFloat($('#cheekWidthSlider').val());
        mp.trigger('creator_setFaceFeature', 10, charData.Face.data[10]);
});
$('#eyeOpenSlider').on('input', 
    function(ev){ 
        charData.Face.data[11] = parseFloat($('#eyeOpenSlider').val());
        mp.trigger('creator_setFaceFeature', 11, charData.Face.data[11]);
});
$('#lipsWideSlider').on('input', 
    function(ev){ 
        charData.Face.data[12] = parseFloat($('#lipsWideSlider').val());
        mp.trigger('creator_setFaceFeature', 12, charData.Face.data[12]);
});
$('#jawWidthSlider').on('input', 
    function(ev){ 
        charData.Face.data[13] = parseFloat($('#jawWidthSlider').val());
        mp.trigger('creator_setFaceFeature', 13, charData.Face.data[13]);
});
$('#jawHeightSlider').on('input', 
    function(ev){ 
        charData.Face.data[14] = parseFloat($('#jawHeightSlider').val());
        mp.trigger('creator_setFaceFeature', 14, charData.Face.data[14]);
});
$('#chinLengthSlider').on('input', 
    function(ev){ 
        charData.Face.data[15] = parseFloat($('#chinLengthSlider').val());
        mp.trigger('creator_setFaceFeature', 15, charData.Face.data[15]);
});
$('#chinPositionSlider').on('input', 
    function(ev){ 
        charData.Face.data[16] = parseFloat($('#chinPositionSlider').val());
        mp.trigger('creator_setFaceFeature', 16, charData.Face.data[16]);
});
$('#chinWidthSlider').on('input', 
    function(ev){ 
        charData.Face.data[17] = parseFloat($('#chinWidthSlider').val());
        mp.trigger('creator_setFaceFeature', 17, charData.Face.data[17]);
});
$('#chinShapeSlider').on('input', 
    function(ev){ 
        charData.Face.data[18] = parseFloat($('#chinWidthSlider').val());
        mp.trigger('creator_setFaceFeature', 18, charData.Face.data[18]);
});
$('#neckWidthSlider').on('input', 
    function(ev){ 
        charData.Face.data[19] = parseFloat($('#neckWidthSlider').val());
        mp.trigger('creator_setFaceFeature', 19, charData.Face.data[19]);
});

function toggleOption(option){
    var formId = `#${option}Form`;
    var optionId = `#${option}Option`;

    if(activeOption == optionId) return;

    $(activeOption).removeClass('Active');
    activeOption = optionId;
    $(activeOption).addClass('Active');
    
    hideForm(activeForm);
    activeForm = formId;
    showForm(activeForm);
}

function toggleFaceOption(option){

    var buttonId = `#${option}Option`;
    var optionId = `#${option}Options`;

    if(activeFaceOption == buttonId) return;

    $(activeFaceOption).removeClass('Active');
    activeFaceOption = buttonId;
    $(activeFaceOption).addClass('Active');
    
    hideForm(activeFaceForm);
    activeFaceForm = optionId;
    showForm(activeFaceForm);
}

function showForm(form) {
    $(form).show();
}
function hideForm(form) {
    $(form).hide();
}
