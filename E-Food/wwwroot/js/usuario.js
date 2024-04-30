let datatable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    datatable = $('#tblDatos').DataTable({
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
            "url": "/Admin/Usuario/ObtenerTodos"
        },
        "columns": [
            { "data": "email" },
            { "data": "userName" },
            { "data": "rol" },
            {
                "data": {
                    id: "id",
                "render": function (data) {
                    return `
                        <div class="text-center">
                           <a href="/Admin/Producto/Upsert/${data}" class="btn btn-success text-white" style="cursor:pointer">
                              <i class="bi bi-pencil-square"></i>  
                           </a>
                        </div>
                    `;
                }, "width": "20%"
                    //let hoy = new Date().getTime();
                    //let bloqueo = new Date(data.lockoutEnd).getTime();
                    //if (bloqueo > hoy) {
                    //    // Usuario esta Bloqueado
                    //    return `
                    //        <div class="text-center">
                    //           <a onclick=BloquearDesbloquear('${data.id}') class="btn btn-danger text-white" style="cursor:pointer", width:150px >
                    //                <i class="bi bi-unlock-fill"></i> Desbloquear
                    //           </a>
                    //        </div>
                    //    `;
                    //}
                    //else {
                    //    return `
                    //        <div class="text-center">
                    //           <a onclick=BloquearDesbloquear('${data.id}') class="btn btn-success text-white" style="cursor:pointer", width:150px >
                    //                <i class="bi bi-lock-fill"></i> Bloquear
                    //           </a>
                    //        </div>
                    //    `;
                    //}


                }
            }
        ]

    });
}

function BloquearDesbloquear(id) {
    $.ajax({
        type: "POST",
        url: '/Admin/Usuario/BloquearDesbloquear',
        data: JSON.stringify(id),
        contentType: "application/json",
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