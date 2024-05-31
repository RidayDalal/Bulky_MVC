/*// Global variable.
var dataTable

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#companyData').DataTable({
        // Retrieves all the records of the companies.
        ajax: { url: '/Admin/Company/getall' }, // API URL
        // Column names must exactly match the name in the database and how it is
        // mentioned when you obtin the data from the above API call (case sensitive).

        // Important: The number of columns here should match the number of <th> tags
        // that are under the <thead> tag in Index.cshtml file.
        columns: [
            { data: 'name', "width": "10%" },
            { data: 'streetAddress', "width" : "20%" },
            { data: 'city', "width": "10%" },
            { data: 'state', "width": "10%" },
            { data: 'postalCode', "width": "10%" },
            { data: 'phoneNumber', "width": "15%" },
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="w-75 btn-group">
                        <a href="/admin/company/upsert?id=${data}" class="btn btn-primary mx-2"> <i class="bi bi-pencil-square"></i> Edit</a>
                        <a onClick=Delete("/admin/company/delete/${data}") class="btn btn-danger mx-2"> <i class="bi bi-trash-fill"></i> Delete</a>
                        
                    </div>`
                },
                "width": "25%"
            },
        ]
    });
}

function Delete(url) {
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        // Here, we perform an Ajax request to the controller to delete the particular company.
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: "DELETE", 
                success: function (data) {
                    // This reloads the page after the record is deleted so that
                    // we are able to notice that change as soon as the record is deleted.
                    dataTable.ajax.reload()
                    // If deleting is successful, then the toastr notification of 
                    // success appears on our screen.
                    toastr.success(data.message)
                }
            })
        }
    });
}
    */



var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/admin/company/getall' },
        "columns": [
            { "data": "name", "width": "15%" },
            { "data": "streetAddress", "width": "15%" },
            { "data": "city", "width": "15%" },
            { "data": "state", "width": "15%" },
            { "data": "phoneNumber", "width": "15%" },
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
                     <a href="/admin/company/upsert?id=${data}" class="btn btn-primary mx-2"> <i class="bi bi-pencil-square"></i> Edit</a>               
                     <a onClick=Delete('/admin/company/delete/${data}') class="btn btn-danger mx-2"> <i class="bi bi-trash-fill"></i> Delete</a>
                    </div>`
                },
                "width": "25%"
            }
        ]
    });
}

function Delete(url) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    dataTable.ajax.reload();
                    toastr.success(data.message);
                }
            })
        }
    })
}