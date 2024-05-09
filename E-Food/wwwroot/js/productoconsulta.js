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
