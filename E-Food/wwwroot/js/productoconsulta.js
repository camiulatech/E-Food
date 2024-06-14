$(document).ready(function () {
    const url = '/Admin/Producto/ObtenerTodos';
});

function filtrarPorLineaComida() {

    $('#frmFiltrar').submit();
}

function limpiarFiltro() {

    document.getElementById("lineaComidaSelect").value = "";

    document.getElementById("frmFiltrar").submit();
 

    $.ajax({
        type: "GET",
        url: "/Admin/Producto/ObtenerTodos",
        success: function (response) {
            var data = response.data;

            datatable.clear().rows.add(data).draw();
        },
        error: function (error) {
            console.log(error);
        }
    });
}
