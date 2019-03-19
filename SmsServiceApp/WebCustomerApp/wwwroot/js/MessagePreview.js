var CursorPosition = 0;
var hashTagWords = ["#name", "#surname", "#birthday", "#company"];
var hashTagWordsRaplacer = ["ClientName", "ClientSurname", "ClientBirthday", "CompanyName"];

function AddWord(word) {
    var v = $('#message').val();
    var textBefore = v.substring(0, CursorPosition);
    var textAfter = v.substring(CursorPosition, v.length);
    var tmpCursorPosition = CursorPosition;

    $('#message').val(textBefore + word + textAfter);
    MessagePreview($('#message'));
    CursorPosition = tmpCursorPosition + word.length;
}

function hashTagWordReplaceByPosition(position, word) {
    var v = $('#message').val();
    var hashTagWordIndex = hashTagWords.findIndex(function (val) { return val == word; });
    var textBefore = v.substring(0, position);
    var textAfter = v.substring(position + word.length + 1, v.length);
    v = textBefore + hashTagWordsRaplacer[hashTagWordIndex] + textAfter;
    $('#messagePreview').val(v);
}

function MessagePreview(item) {
    CursorPosition = $(item).prop('selectionStart');
    var v = $(item).val();
    for (var i = 0; i < hashTagWords.length; i++) {
        v = v.replace(new RegExp(hashTagWords[i], 'g'), hashTagWordsRaplacer[i]);
    }
    $('#messagePreview').text(v);
    if (v.length == 0) {
        $('#messagePreview')[0].style.display = "none";
    }
    else {
        $('#messagePreview')[0].style.display = "block";
    }
    $('#messagePreview')[0].style.height = "0px";
    $('#messagePreview')[0].style.height = $('#messagePreview')[0].scrollHeight + "px";
}

function ClickPosition(item) {
    CursorPosition = $(item).prop('selectionStart');
}

function MessagePreviewShow(item) {
    $('#messagePreviewContainer')[0].style.width = "320px";
    $('#messagePreview')[0].style.height = "0px";
    $('#messagePreview')[0].style.height = $('#messagePreview')[0].scrollHeight + "px";
    item.setAttribute("onclick", "MessagePreviewHide(this)");
}

function MessagePreviewHide(item) {
    $('#messagePreviewContainer')[0].style.width = "0px";
    item.setAttribute("onclick", "MessagePreviewShow(this)");
}