function add_anti_forgery_token(data) {
    data.__RequestVerificationToken = $('[name=__RequestVerificationToken]').val();
    return data;
}

function formatar_mensagem_aviso(mensagens) {
    var ret = '';
    for (var i = 0; i < mensagens.length; i++) {
        ret += '<li>' + mensagens[i] + '</li>';
    }
    return '<ul>' + ret + '</ul>';
}

function abrir_form(dados) {

    // Preencher os campos
    set_dados_form(dados);

    // Formulario de Cadastro
    var modal_cadastro = $('#modal_cadastro');

    $('#msg_mensagem_aviso').empty();
    $('#msg_aviso').hide();
    $('#msg_mensagem_aviso').hide();
    $('#msg_erro').hide();

    bootbox.dialog({
        title: 'Cadastro de '+tituloPagina,
        message: modal_cadastro
    })
        .on('shown.bs.modal', function () {
            modal_cadastro.show(0, function () {   // mostrar o formulario - até aqui vai funcionar apenas na primeira vez, precisa implementar
                set_focus_form();                  // o evento hidden.bs.model, ocultando e inserindo no corpo
            });
        })
        .on('hidden.bs.modal', function () {
            modal_cadastro.hide().appendTo('body');
        });
}


function criar_linha_grid(dados) {
    var ret =
        '<tr data-id=' + dados.Id + '>' +
        set_dados_grid(dados) +
        '<td>' +
        '<a class="btn btn-primary btn-alterar" role="button" style="margin-right: 3px"><i class="glyphicon glyphicon-pencil"></i>Alterar</a>' +
        '<a class="btn btn-danger btn-excluir" role="button"><i class="glyphicon glyphicon-trash"></i>Excluir</a>' +
        '</td>' +
        '</tr>';

    return ret;
}
$(document)
    .on('click', '#btn_incluir', function () {  // Classe #btn_incluir
        abrir_form(get_dados_inclusao()); // como a função exige o parametro, deve ser informado
    })
    .on('click', '.btn-alterar', function () {  // nome btn_alterar
        var btn = $(this),
            id = btn.closest('tr').attr('data-id'),  // foi preciso inserir no elemento TR em ForEach para buscar a informação do ID na alteracao
            url = url_alterar,
            param = { 'id': id };

        $.post(url, add_anti_forgery_token(param), function (response) {
            if (response) {
                abrir_form(response);  // Se retornou positivo, traz os dados para dentro da função abrir_form(dados)
            }
        });
    })
    .on('click', '.btn-excluir', function () {  // nome btn_excluir
        var btn = $(this),
            tr = btn.closest('tr'),
            id = tr.attr('data-id'),  // foi preciso inserir no elemento TR em ForEach para buscar a informação do ID na alteracao
            url = url_excluir,
            param = { 'id': id };

        bootbox.confirm({
            message: "Realmente deseja excluir o " + tituloPagina + "?",
            buttons: {
                confirm: {
                    label: 'Sim',
                    className: 'btn-danger'
                },
                cancel: {
                    label: 'Não',
                    className: 'btn-success'
                }
            },
            callback: function (result) {
                if (result) { // Caso o resultado for Sim
                    $.post(url, add_anti_forgery_token(param), function (response) {  // Executa a remoção na Controller
                        if (response) {
                            tr.remove();  // Remover a linha na tabela. O Efeito da remoção é visto imediatamente, sem precisar atualizar a pagina.
                        }
                    });

                }
            }
        });
    })
    .on('click', '#btn_confirmar', function () {  // Ação do botão Salvar do formulario
        var btn = $(this),
            url = url_confirmar,
            param = get_dados_form(); // Informando os dados para enviar para a controller inserir ou alterar.            

        $.post(url, add_anti_forgery_token(param), function (response) {
            if (response.Resultado == "OK") {
                if (param.Id == 0) { // inclusao
                    param.Id = response.IdSalvo; // Recupera o valor de ID salvo
                    var table = $('#grid_cadastro').find('tbody'),
                        linha = criar_linha_grid(param);

                    table.append(linha);
                }
                else {
                    var linha = $('#grid_cadastro').find('tr[data-id=' + param.Id + ']').find('td');

                    preencher_linha_grid(param,linha);
                }

                $('#modal_cadastro').parents('.bootbox').modal('hide'); // Fecha o formulario de cadastro
            }
            else if (response.Resultado == "ERRO") {
                $('#msg_aviso').hide();
                $('#msg_mensagem_aviso').hide();
                $('#msg_erro').show();

            } else if (response.Resultado == "AVISO") {
                $('#msg_mensagem_aviso').html(formatar_mensagem_aviso(response.Mensagens));
                $('#msg_aviso').show();
                $('#msg_mensagem_aviso').show();
                $('#msg_erro').hide();
            }
        });
    })
    .on('click', '.page-item', function () {
        var btn = $(this),
            pagina = btn.text(),
            tamPag = $('#ddl_tam_pag').val(),
            url = url_page_click,
            param = { 'pagina': pagina, 'tamPag': tamPag };

        $.post(url, add_anti_forgery_token(param), function (response) {
            if (response) {   // apagando e poplulando a lista da table

                var table = $('#grid_cadastro').find('tbody');

                table.empty();

                for (var i = 0; i < response.length; i++) {
                    table.append(criar_linha_grid(response[i]));                    
                }

                btn.siblings().removeClass('active');
                btn.addClass('active');
            }
        })
    })
    .on('change', '#ddl_tam_pag', function () {
        var ddl = $(this),
            tamPag = ddl.val(),
            pagina = 1,
            url = url_tam_pag_change,
            param = { 'pagina': pagina, 'tamPag': tamPag };

        $.post(url, add_anti_forgery_token(param), function (response) {
            if (response) {   // apagando e poplulando a lista da table

                var table = $('#grid_cadastro').find('tbody');
                table.empty();

                for (var i = 0; i < response.length; i++) {
                    table.append(criar_linha_grid(response[i]));
                }

                ddl.siblings().removeClass('active');
                ddl.addClass('active');
            }
        })
    });

