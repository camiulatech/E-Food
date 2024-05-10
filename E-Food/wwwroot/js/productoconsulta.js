$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    $('#tblDatos').DataTable({
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
            { "data": "lineaComida.nombre", "width": "20%" }
            // Aquí especificamos que queremos mostrar el precio
        ]
    });
}


function filtrarPorLineaComida() {
    var idLineaComida = document.getElementById("lineaComidaSelect").value;
    // Envía el formulario al controlador con el ID de la línea de comida seleccionada
    document.getElementById("frmFiltrar").submit();
}

function limpiarFiltro() {

        // Limpiar el valor seleccionado en el select de línea de comida
    document.getElementById("lineaComidaSelect").value = "";

        // Enviar el formulario vacío para obtener todos los productos nuevamente
    document.getElementById("frmFiltrar").submit();
 

    // Realizar una solicitud AJAX para obtener todos los registros nuevamente
    $.ajax({
        type: "GET",
        url: "/Admin/Producto/ObtenerTodos",
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
