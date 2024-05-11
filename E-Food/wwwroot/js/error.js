let datatable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    datatable = $('#tblDatos').DataTable({
        "language": {
            "lengthMenu": "Mostrar _MENU_ Registros Por Página",
            "zeroRecords": "Ningún Registro",
            "info": "Mostrar página _PAGE_ de _PAGES_",
            "infoEmpty": "No hay registros",
            "infoFiltered": "(filtrado de _MAX_ registros totales)",
            "search": "Buscar",
            "paginate": {
                "first": "Primero",
                "last": "Último",
                "next": "Siguiente",
                "previous": "Anterior"
            }
        },
        "ajax": {
            "url": "/Admin/Error/ObtenerTodos"
        },
        "columns": [
            { "data": "id", "width": "10%" },
            {
                "data": "fecha", "width": "15%",
                "render": function (data) {
                    var fecha = new Date(data);
                    var options = { year: 'numeric', month: 'short', day: 'numeric' };
                    return fecha.toLocaleDateString('es-ES', options);
                }
            },
            {"data": "hora", "width": "15%" },
            {"data": "numeroError", "width": "15%"},
            {"data": "mensaje", "width": "45%" },
        ]
    });
}

function filtrarPorFecha() {
    var fecha = $('#fecha').val();

    $.ajax({
        type: "GET",
        url: "/Admin/Error/ObtenerPorFecha",
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
        url: "/Admin/Error/ObtenerTodos",
        success: function (response) {
            var data = response.data;
            datatable.clear().rows.add(data).draw();
        },
        error: function (error) {
            console.log(error);
        }
    });
}
