let datatable;

$(document).ready(function () {
    const id = obtenerUltimoID();

    loadDataTable(id);
    loadDataTable1(id);
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
            "url": `/Admin/ProcesadorTarjeta/ObtenerTodos/${id}`
        },
        "columns": [
            { "data": "id", "width": "20%" },
            { "data": "nombre", "width": "60%"},
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

function loadDataTable1(id) {
    datatable1 = $('#tblDatos1').DataTable({
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
            "url": `/Admin/ProcesadorTarjeta/ObtenerNoAsociados/${id}`
        },
        "columns": [
            { "data": "id", "width": "20%" },
            { "data": "nombre", "width": "60%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                    <div class="text-center">
                           <a onclick=Upsert(${data}) class="btn btn-success text-white" style="cursor:pointer">
                                <i class="bi bi-plus-circle-fill"></i>
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
    const url = `/Admin/ProcesadorTarjeta/Eliminar/${val}`
    swal({
        title: "Esta seguro de Eliminar la Tarjeta?",
        text: "Sea cuidadoso con su seleccion!",
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
                        datatable1.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });


}

function Upsert(id1) {
    const id = obtenerUltimoID();
    const val = `${id}, ${id1}`
    const url = `/Admin/ProcesadorTarjeta/Upsert/${val}`
    swal({
        title: "Esta seguro de Agregar la Tarjeta?",
        text: "Si esta seguro continue",
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
                        datatable1.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });


}