document.addEventListener("DOMContentLoaded", function () {
    
    const inputCep = document.getElementById("CepAluno");

    if (inputCep) {
        inputCep.addEventListener("blur", async function (e) {
            
            let cep = e.target.value.replace(/\D/g, "");

            if (cep.length === 8) {
                try {
                    const resposta = await fetch(`https://viacep.com.br/ws/${cep}/json/`);
                    const dados = await resposta.json();

                    if (dados.erro) {
                        alert("CEP não encontrado. Verifique o número digitado.");
                        return; 
                    }

                    document.getElementById("RuaAluno").value = dados.logradouro;
                    document.getElementById("BairroAluno").value = dados.bairro;
                    document.getElementById("CidadeAluno").value = dados.localidade;
                    document.getElementById("EstadoAluno").value = dados.uf;

                } catch (erro) {
                    console.error("Erro ao buscar o CEP:", erro);
                    alert("Ocorreu um erro ao consultar o CEP. Tente novamente mais tarde.");
                }
            }
        });
    }
});