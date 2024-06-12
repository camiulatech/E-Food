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
            "url": "/Admin/Pedido/ObtenerTodos"
        },
        "columns": [
            { "data": "id", "width": "10%" },
            {
                "data": "fecha",
                "width": "15%",
                "render": function (data) {
                    // Formatea la fecha utilizando JavaScript
                    var fecha = new Date(data);
                    var options = { year: 'numeric', month: 'short', day: 'numeric' };
                    return fecha.toLocaleDateString('es-ES', options);
                }
            },
            { "data": "monto", "width": "15%" },
            {
                "data": "estado",
                "width": "10%",
                "render": function (data) {
                    switch (data) {
                        case 0:
                            return 'Procesado';
                        case 1:
                            return 'Cancelado';
                        case 2:
                            return 'EnCurso';
                        default:
                            return '';
                    }
                }
            },
            { "data": "tiqueteDescuento.codigo", "width": "10%" },
            { "data": "procesadorPago.procesador", "width": "15%" },
            {
                "data": "productos",
                "width": "30%",
                "render": function (data) {
                    return data.map(p => p.nombre).join(', ');
                }
            }
        ]
    });
}

function filtrarPorFecha() {
    var fecha = $('#fecha').val(); // Obtener la fecha seleccionada por el usuario

    // Realizar una solicitud AJAX para obtener los registros filtrados por fecha
    $.ajax({
        type: "GET",
        url: "/Admin/Pedido/ObtenerPorFecha",
        data: { fecha: fecha },
        success: function (response) {
            var data = response.data; // Extrae los datos del objeto de respuesta

            // Reemplaza los datos actuales en la tabla con los nuevos datos
            datatable.clear().rows.add(data).draw();
        },
        error: function (error) {
            console.log(error);
        }
    });
}

function limpiarFiltro() {
    $('#fecha').val(''); // Limpiar el valor del campo de fecha

    // Realizar una solicitud AJAX para obtener todos los registros nuevamente
    $.ajax({
        type: "GET",
        url: "/Admin/Pedido/ObtenerTodos",
        success: function (response) {
            var data = response.data; // Extrae los datos del objeto de respuesta

            // Reemplaza los datos actuales en el DataTable con los nuevos datos
            datatable.clear().rows.add(data).draw();
        },
        error: function (error) {
            console.log(error);
        }
    });
}
