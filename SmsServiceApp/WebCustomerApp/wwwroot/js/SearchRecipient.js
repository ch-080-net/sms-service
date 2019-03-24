function FilterRecipient() {
    var input, filter, tbody, tr, tdItem, txtValue;
    var isShow = false;
    input = document.getElementById("inputTable");
    filter = input.value.toUpperCase();
    tbody = document.getElementById('tableR');
    tr = tbody.getElementsByTagName('tr');
    
    for (var i = 0; i < tr.length; i++) {
        
        for (var j = 0; j < 7; j++) {
            tdItem = tr[i].getElementsByTagName('td')[j];
            txtValue = (tdItem.textContent) || (tdItem.innerText)

            if ((txtValue.toUpperCase().indexOf(filter) > -1)) {
                isShow = true;
            }
        }
       
        if (isShow){
            tr[i].style.display = "";
        }
        else {
            tr[i].style.display = "none";
        }
    }

}