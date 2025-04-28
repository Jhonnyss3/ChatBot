using ChatBot.Domain.Interfaces;

namespace ChatBot.Application.Services
{
    public class ContextualChatResponder : IChatResponder
    {
        private readonly IChatSessionService _sessionService;

        public ContextualChatResponder(IChatSessionService sessionService)
        {
            _sessionService = sessionService;
        }

        public async Task<string> RespondAsync(string message, string? sessionId = null)
        {
            if (string.IsNullOrEmpty(sessionId))
                sessionId = Guid.NewGuid().ToString();

            var session = await _sessionService.GetSessionAsync(sessionId);
            var intent = DetectIntent(message.ToLower());

            string response;

            if (session.State == "inicio" && intent != "encerrar")
            {
                session.State = "historia";
                session.LastTopic = "historia";
                response = "E ai Furioso, sou o Pantera, chatbot oficial da Fúria E-sports. Aqui estão os assuntos que podemos falar sobre o nosso time de CS:\n" +
                           "Nossa história\n" +
                           "Nossos jogadores\n" +
                           "Nossas redes sociais\n" +
                           "Onde assisto a Fúria?\n" +
                           "Onde compro materiais oficiais da Fúria?\n" +
                           "Ou digite 'Valeu Pantera' para encerrarmos o chat.";
            }
            else if (session.State == "historia" && intent == "nossa historia")
            {
                session.State = "titulos";
                response = "A FURIA Esports foi fundada em 2017 com a missão de se tornar uma potência no cenário de CS:GO... [informações sobre história]\n" +
                           "Caso queira saber mais sobre nossos Títulos, digite 'Títulos', ou 'Voltar' para voltar ao menu principal.";
            }
            else if (session.State == "titulos" && intent == "titulos")
            {
                session.State = "titulos_lista";
                response = "A FURIA Esports conquistou muitos momentos marcantes no CS:GO... [informações sobre títulos]\n" +
                           "Agora, digite o que deseja continuar sabendo sobre a Fúria:\n" +
                           "Nossos Jogadores\nNossas Redes Sociais\nOnde assisto a Fúria?\nOnde compro materiais oficiais da Fúria?";
            }
            else if (session.State == "titulos_lista" && intent == "jogadores")
            {
                session.State = "jogadores";
                response = "A FURIA Esports segue forte no cenário internacional de CS:GO com uma line-up renovada...\n" +
                           "Agora digite o que deseja continuar sabendo sobre a Fúria:\n" +
                           "Nossa história\nNossas Redes Sociais\nOnde assisto a Fúria?\nOnde compro materiais oficiais da Fúria?";
            }
            else if (session.State == "jogadores" && intent == "redes sociais")
            {
                session.State = "redes_sociais";
                response = "Você vai encontrar tudo sobre a Fúria aqui:\n" +
                           "Twitter: @FURIA\nInstagram: @furiagg\nYouTube: FURIA\nTwitch: FURIAtv\nTikTok: @furiagg\nSite: furia.gg\n" +
                           "Agora digite o que deseja continuar sabendo sobre a Fúria:\n" +
                           "Nossa história\nNossos Jogadores\nOnde assisto a Fúria?\nOnde compro materiais oficiais da Fúria?";
            }
            else if (session.State == "redes_sociais" && intent == "onde assistir")
            {
                session.State = "assistir_furia";
                response = "Acompanhe os jogos da Fúria nos canais:\n" +
                           "YouTube: https://www.youtube.com/@FURIAggCS\nTwitch: https://www.twitch.tv/furiatv\n" +
                           "Agora digite o que deseja continuar sabendo sobre a Fúria:\n" +
                           "Nossa história\nNossos Jogadores\nNossas Redes Sociais\nOnde compro materiais oficiais da Fúria?";
            }
            else if (session.State == "assistir_furia" && intent == "comprar")
            {
                session.State = "comprar_material";
                response = "Compre produtos oficiais da Fúria:\nhttps://www.furia.gg/\n" +
                           "Agora digite o que deseja continuar sabendo sobre a Fúria:\n" +
                           "Nossa história\nNossos Jogadores\nNossas Redes Sociais\nOnde assisto a Fúria?";
            }
            else if (intent == "encerrar")
            {
                session.State = "encerrado";
                response = "Eu que agradeço e espero que você tenha encontrado tudo que precisa! GO FÚRIA!";
            }
            else
            {
                response = "Não entendi, pode repetir a informação que deseja saber sobre a Fúria CS?";
            }

            return response;
            
        }

        private string DetectIntent(string input)
        {
            if (input.Contains("história") || input.Contains("sobre a furia"))
                return "nossa historia";

            if (input.Contains("título") || input.Contains("campeonato"))
                return "titulos";

            if (input.Contains("jogador") || input.Contains("lineup") || input.Contains("equipe"))
                return "jogadores";

            if (input.Contains("rede") || input.Contains("instagram") || input.Contains("twitter") || input.Contains("tiktok"))
                return "redes sociais";

            if (input.Contains("assist") || input.Contains("ver") || input.Contains("transmissão"))
                return "onde assistir";

            if (input.Contains("comprar") || input.Contains("loja") || input.Contains("material"))
                return "comprar";

            if (input.Contains("valeu pantera") || input.Contains("encerrar") || input.Contains("tchau"))
                return "encerrar";

            return input;
        }
    }
}
