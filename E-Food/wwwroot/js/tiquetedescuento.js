let datatable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    datatable = $('#tblDatos').DataTable({
        "language": {
            "lengthMenu": "Mostrar _MENU_ Registros Por Pagina",
            "zeroRecords": "Ningun Registro",
            "info": "Mostrar page _PAGE_ de _PAGES_",
            "infoEmpty": "no hay registros",
            "infoFiltered": "(filtered from _MAX_ total registros)",
            "search": "Buscar",
            "paginate": {
                "first": "Primero",
                "last": "Último",
                "next": "Siguiente",
                "previous": "Anterior"
            }
        },
        "ajax": {
            "url": "/Admin/TiqueteDescuento/ObtenerTodos"
        },
        "columns": [
            { "data": "id", "width": "16%" },
            { "data": "codigo", "width": "16%"},
            { "data": "descripcion", "width": "16%" },
            { "data": "disponibles", "width": "16%" },
            { "data": "descuento", "width": "16%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="text-center">
                           <a href="/Admin/TiqueteDescuento/Upsert/${data}" class="btn btn-success text-white" style="cursor:pointer">
                              <i class="bi bi-pencil-square"></i>  
                           </a>
                           <a onclick=Eliminar("/Admin/TiqueteDescuento/Eliminar/${data}") class="btn btn-danger text-white" style="cursor:pointer">
                                <i class="bi bi-trash3-fill"></i>
                           </a> 
                        </div>
                    `;
                }, "width": "16%"
            }
        ]

    });
}

function Eliminar(url) {
    swal({
        title: "Esta seguro de Eliminar el Tiquete de Descuento?",
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