$(document).ready(function () {
    GetTopAccounts();

});

function GetTopAccounts() {
    // TODO Fix Url to be relative
    var URL = "https://localhost:7207/api/Account";
    $.ajax({
        method: "GET",
        cache: false,
        url: URL,
        success: function (data, status) {
            // Get a reference to the table element
            const table = document.getElementById('accounts');
            for (let i = 0; i < data.length; i++) {
                const newRow = table.insertRow();
                // Insert cells into the new row
                const cell1 = newRow.insertCell();
                const cell2 = newRow.insertCell();
                // Set the values of the cells
                
                cell1.innerHTML = data[i].topAccount;
                cell2.innerHTML = data[i].totalBalance;

                newRow.addEventListener("click", function () {
                   
                    GetDetilsForAccount(data[i].topAccount);
                });
                
            }
        },
        error: function (xhr, status, error) {
            console.log(error);
        }
    });
}
function GetDetilsForAccount(id) {
    // TODO Fix Url to be relative
    var URL = "https://localhost:7207/api/Account/"+id;
    $.ajax({
        method: "GET",
        cache: false,
        url: URL,
        success: function (data, status) {
            let details = "";
            for (let i = 0; i < data.length; i++) {
                let _path = "Account " + (data[i].path) + " = " + (data[i].balance).toString();
                details += _path + "\n";
            }
            alert(details);
        },
        error: function (xhr, status, error) {
            console.log(error);
        }
    });
}
