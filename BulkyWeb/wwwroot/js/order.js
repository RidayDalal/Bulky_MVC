// Global variable.
var dataTable

$(document).ready(function () {
    var url = window.location.search;
    if (url.includes("inprocess")) {
        loadDataTable("inprocess")
    } else if (url.includes("completed")) {
        loadDataTable("completed")
    } else if (url.includes("pending")) {
        loadDataTable("pending")
    } else if (url.includes("approved")) {
        loadDataTable("approved")
    } else {
        loadDataTable("all")
    }
});

function loadDataTable(status) {
    dataTable = $('#tblData').DataTable({
        // Retrieves all the records of the products.
        ajax: { url: '/Admin/Order/getall?status=' + status }, // API URL
        // Column names must exactly match the name in the database and how it is
        // mentioned when you obtin the data from the above API call (case sensitive).

        // Important: The number of columns here should match the number of <th> tags
        // that are under the <thead> tag in Index.cshtml file.
        columns: [
            { data: 'id', "width": "25%" },
            { data: 'name', "width": "20%" },
            { data: 'phoneNumber', "width": "20%" },
            { data: 'applicationUser.email', "width": "20%" },
            { data: 'orderStatus', "width": "10%" },
            { data: 'orderTotal', "width": "10%" },
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="w-75 btn-group">
                        <a href="/admin/order/details?orderId=${data}" class="btn btn-primary mx-2"> <i class="bi bi-pencil-square"></i> Edit</a>
                        </div>`
                },
                "width": "10%"
            },
        ]
    });
}