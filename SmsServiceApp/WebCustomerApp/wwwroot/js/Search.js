function FilterNameDesc() {
    var input, filter, tbody, tr, tdName, tdDescription, txtValueName, txtValueDescription;
    input = document.getElementById("inputTable");
    filter = input.value.toUpperCase();
    tbody = document.getElementById('tables')
    tr = tbody.getElementsByTagName('tr')
    for (var i = 0; i < tr.length; i++) {
        tdName = tr[i].getElementsByTagName('td')[0];
        tdDescription = tr[i].getElementsByTagName('td')[1];

        txtValueName = (tdName.textContent) || (tdName.innerText);
        txtValueDescription = (tdDescription.textContent) || (tdDescription.innerText);

        if ((txtValueName.toUpperCase().indexOf(filter) > -1) || (txtValueDescription.toUpperCase().indexOf(filter)>-1))
        {
            tr[i].style.display = "";
        }
        else {
            tr[i].style.display="none";
        }
    }

}