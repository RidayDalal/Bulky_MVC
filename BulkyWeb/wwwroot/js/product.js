// Global variable.
var dataTable

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        // Retrieves all the records of the products.
        ajax: { url: '/Admin/Product/getall' }, // API URL
        // Column names must exactly match the name in the database and how it is
        // mentioned when you obtin the data from the above API call (case sensitive).

        // Important: The number of columns here should match the number of <th> tags
        // that are under the <thead> tag in Index.cshtml file.
        columns: [
            { data: 'title', "width": "25%" },
            { data: 'isbn', "width" : "15%" },
            { data: 'listPrice', "width": "10%" },
            { data: 'author', "width": "15%" },
            { data: 'category.name', "width": "10%" },
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="w-75 btn-group">
                        <a href="/admin/product/upsert?id=${data}" class="btn btn-primary mx-2"> <i class="bi bi-pencil-square"></i> Edit</a>
                        <a onClick=Delete("/admin/product/delete/${data}") class="btn btn-danger mx-2"> <i class="bi bi-trash-fill"></i> Delete</a>
                        
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
        // Here, we perform an Ajax request to the controller to delete the particular product.
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
    