let datatable;

$(document).ready(function () {
    const id = obtenerUltimoID();

    loadDataTable(id);
});

function loadDataTable(id) {
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
            "url": `/Admin/ProductoPrecio/ObtenerTodos/${id}`
        },
        "columns": [
            { "data": "id", "width": "20%" },
            { "data": "productoId", "width": "20%"},
            { "data": "descripcion", "width": "20%" },
            { "data": "monto", "width": "20%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                    <div class="text-center">
                           <a onclick=Eliminar(${data}) class="btn btn-danger text-white" style="cursor:pointer">
                                <i class="bi bi-trash3-fill"></i>
                           </a> 
                        </div>
                    `;
                }, "width": "20%"
            }
        ]

    });
}

function obtenerUltimoID() {
    const url = window.location.href;
    const partesURL = url.split('/');
    return partesURL[partesURL.length - 1];
}

function Eliminar(id1) {
    const id = obtenerUltimoID();
    const val = `${id}, ${id1}`
    const url = `/Admin/ProductoPrecio/Eliminar/${val}`
    swal({
        title: "Esta seguro de Eliminar el Precio?",
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