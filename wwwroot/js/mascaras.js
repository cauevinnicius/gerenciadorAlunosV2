// espera a página carregar inteira antes de tentar procurar os campos
document.addEventListener("DOMContentLoaded", function () {
    
    // crio uma variavel para buscar o input do CPF pelo seu Id
    const inputCpf = document.getElementById("CpfAluno"); 

    if (inputCpf) {
        
        // capturo o valor do campo a cada digitação do usuário e aplico a máscara
        inputCpf.addEventListener("input", function (e) {
            let valor = e.target.value;

            // Regex: \D significa "Tudo que NÃO for número". O replace troca letras por Vazio ("").
            valor = valor.replace(/\D/g, "");

            // Limita a 11 números para não bugar a máscara
            if (valor.length > 11) {
                valor = valor.substring(0, 11);
            }

            // Coloca o primeiro ponto: 111.22233344
            valor = valor.replace(/(\d{3})(\d)/, "$1.$2");
            
            // Coloca o segundo ponto: 111.222.33344
            valor = valor.replace(/(\d{3})(\d)/, "$1.$2");
            
            // Coloca o traço: 111.222.333-44
            valor = valor.replace(/(\d{3})(\d{1,2})$/, "$1-$2");

            // Devolve o valor formatado para o campo na tela
            e.target.value = valor;
        });
    }

    const inputCelular = document.getElementById("CelularAluno");

    if (inputCelular) {
        inputCelular.addEventListener("input", function (e) {
        let celular = e.target.value;

        // tiro tudo que não for número
        celular = celular.replace(/\D/g, "");

        // limito a 11 números
        if (celular.length > 11) {
            celular = celular.substring(0, 11);
        }

        // coloco o parênteses do DDD logo após o segundo número
        celular = celular.replace(/^(\d{2})(\d)/g, "($1) $2");

        // coloco o traço antes dos últimos 4 números 
        celular = celular.replace(/(\d)(\d{4})$/, "$1-$2");

        e.target.value = celular;
        });
    }
});