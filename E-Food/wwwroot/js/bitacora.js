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
            "url": "/Admin/Bitacora/ObtenerTodos"
        },
        "columns": [
            { "data": "id", "width": "10%" },
            { "data": "usuario", "width": "15%" },
            {
                "data": "fecha",
                "width": "15%",
                "render": function (data) {
                    var fecha = new Date(data);
                    var options = { year: 'numeric', month: 'short', day: 'numeric' };
                    return fecha.toLocaleDateString('es-ES', options);
                }
            },
            { "data": "codigoRegistro", "width": "10%" },
            { "data": "descripcion", "width": "30%" },

        ]
    });
}

function filtrarPorFecha() {
    var fecha = $('#fecha').val();

    $.ajax({
        type: "GET",
        url: "/Admin/Bitacora/ObtenerPorFecha",
        data: { fecha: fecha },
        success: function (response) {
            var data = response.data;
            datatable.clear().rows.add(data).draw();
        },
        error: function (error) {
            console.log(error);
        }
    });
}

function limpiarFiltro() {
    $('#fecha').val('');

    $.ajax({
        type: "GET",
        url: "/Admin/Bitacora/ObtenerTodos",
        success: function (response) {
            var data = response.data;
            datatable.clear().rows.add(data).draw();
        },
        error: function (error) {
            console.log(error);
        }
    });
}

