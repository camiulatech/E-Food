let datatable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    datatable = $('#tblDatos').DataTable({
        "language": {
            "lengthMenu": "Mostrar MENU Registros Por Pagina",
            "zeroRecords": "Ningun Registro",
            "info": "Mostrar page PAGE de PAGES",
            "infoEmpty": "no hay registros",
            "infoFiltered": "(filtered from MAX total registros)",
            "search": "Buscar",
            "paginate": {
                "first": "Primero",
                "last": "Último",
                "next": "Siguiente",
                "previous": "Anterior"
            }
        },
        "ajax": {
            "url": "/Admin/Producto/ObtenerTodos"
        },
        "columns": [
            { "data": "id", "width": "20%" },
            { "data": "nombre", "width": "20%" },
            { "data": "lineaComida.nombre", "width": "20%" },
            { "data": "contenido", "width": "20%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="text-center">
                           <a href="/Admin/Producto/Upsert/${data}" class="btn btn-success text-white" style="cursor:pointer">
                              <i class="bi bi-pencil-square"></i>  
                           </a>
                           <a onclick=Eliminar("/Admin/Producto/Eliminar/${data}") class="btn btn-danger text-white" style="cursor:pointer">
                                <i class="bi bi-trash3-fill"></i>
                           </a> 
                        </div>
                    `;
                }, "width": "20%"
            }
        ]

    });
}

function Eliminar(url) {
    swal({
        title: "Esta seguro de Eliminar el Producto?",
        text: "Este proceso es irreversible!",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((erase) => {
        if (erase) {
            $.ajax({
                type: "POST",
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        datatable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });


}