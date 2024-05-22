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
            { "data": "id", "width": "10%" }, // Ajusta el ancho según tus necesidades
            { "data": "usuario", "width": "15%" }, // Ajusta el ancho según tus necesidades
            {
                "data": "fecha",
                "width": "15%", // Ajusta el ancho según tus necesidades
                "render": function (data) {
                    // Formatea la fecha utilizando JavaScript
                    var fecha = new Date(data);
                    var options = { year: 'numeric', month: 'short', day: 'numeric' };
                    return fecha.toLocaleDateString('es-ES', options);
                }
            },
            { "data": "codigoRegistro", "width": "10%" }, // Ajusta el ancho según tus necesidades
            { "data": "descripcion", "width": "30%" }, // Ajusta el ancho según tus necesidades

        ]
    });
}

function filtrarPorFecha() {
    var fecha = $('#fecha').val(); // Obtener la fecha seleccionada por el usuario

    // Realizar una solicitud AJAX para obtener los registros filtrados por fecha
    $.ajax({
        type: "GET",
        url: "/Admin/Bitacora/ObtenerPorFecha",
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
        url: "/Admin/Bitacora/ObtenerTodos",
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

