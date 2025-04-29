using System.Globalization;
using System.Text;
using ChatBot.Domain.Interfaces;
using System.Text.RegularExpressions;

namespace ChatBot.Application.Services
{
    public class ContextualChatResponder : IChatResponder
    {
        private readonly IChatSessionService _sessionService;

        public ContextualChatResponder(IChatSessionService sessionService)
        {
            _sessionService = sessionService;
        }

        private enum Intent
        {
            None = 0,
            Historia = 1,
            Titulos = 2,
            Jogadores = 3,
            RedesSociais = 4,
            OndeAssistir = 5,
            Comprar = 6,
            Encerrar = 7
        }

        public async Task<string> RespondAsync(string message, string? sessionId = null)
        {
            if (string.IsNullOrEmpty(sessionId))
                sessionId = Guid.NewGuid().ToString();

            var session = await _sessionService.GetSessionAsync(sessionId);

            var normalizedMessage = RemoveDiacritics(message.ToLower()).Trim();
            var intent = DetectIntent(normalizedMessage);

            // Se for encerrar, trata normalmente
            if (intent == Intent.Encerrar)
            {
                session.State = "encerrado";
                await _sessionService.RemoveSessionAsync(session.SessionId);
                return "Eu que agradeço e espero que você tenha encontrado tudo que precisa! GO FÚRIA!";
            }

            // Se for um intent conhecido, responde independente do estado
            switch (intent)
            {
                case Intent.Historia:
                    session.State = "titulos";
                    return "A FURIA Esports foi fundada em 2017 com a missão de se tornar uma potência no cenário de CS:GO. Desde o começo, a equipe se destacou por sua energia única e um estilo de jogo agressivo, ganhando rapidamente espaço no Brasil e no mundo.\r\nEm 2019, o time brilhou ao derrotar grandes nomes, como a Astralis, e se consolidou como uma das principais equipes do Brasil. A chegada de FalleN em 2023 trouxe ainda mais poder ao nosso elenco, aumentando a sede por títulos e a ambição de continuar sendo referência no cenário internacional.\r\nHoje, a FURIA segue firme, representando o Brasil no mundo do CS:GO e mostrando sua garra em cada torneio.\nQuer saber mais sobre nossos títulos? Digite 'Títulos'!";
                case Intent.Titulos:
                    session.State = "titulos_lista";
                    return "A FURIA Esports conquistou muitos momentos marcantes no CS:GO desde sua ascensão ao topo do cenário brasileiro até as vitórias em grandes torneios internacionais. Em 2019, o time já mostrava a que veio, vencendo a DreamHack Masters Dallas e surpreendendo o mundo ao derrotar gigantes como a Astralis.\r\n\r\nEm 2020, a FURIA não parou e garantiu o título da ESL Pro League Season 12: North America, consolidando seu nome entre os melhores. Em 2021, o time continuou em alta, levantando a taça da IEM New York e conquistando a BLAST Premier: Spring American Finals.\r\n\r\nEsses títulos não só solidificaram a FURIA como uma potência no CS, mas também a colocaram como referência para outras equipes brasileiras e internacionais. Com uma trajetória de consistência, a FURIA segue sendo uma das maiores organizações do CS:GO, com seus olhos sempre voltados para novos desafios e conquistas.\nAgora, digite o que deseja continuar sabendo sobre a Fúria:\nJogadores\nRedes sociais\nAssistir\nComprar";
                case Intent.Jogadores:
                    session.State = "jogadores";
                    return "A FURIA Esports segue forte no cenário internacional de CS:GO com uma line-up renovada, destacando-se como uma das principais equipes brasileiras. A formação atual é composta por jogadores experientes e novos talentos:\r\n\r\nGabriel \"FalleN\" Toledo (@fallen): AWPer / IGL\r\n\r\nKaike \"KSCERATO\" Cerato (@kscerato): Rifler\r\n\r\nYuri \"yuurih\" Santos (@yuurihfps): Rifler\r\n\r\nDanil \"molodoy\" Golubenko: Sniper\r\n\r\nMareks \"YEKINDAR\" Gaļinskis(@yek1ndar): Rifler(Stand-In)\r\n\r\nCoach: Sidnei \"sidde\" (@siddecs)\nAgora digite o que deseja continuar sabendo sobre a Fúria:\nHistória\nRedes sociais\nAssistir\nComprar";
                case Intent.RedesSociais:
                    session.State = "redes_sociais";
                    return "Você vai encontrar tudo sobre a Fúria aqui:\nTwitter: @FURIA\nInstagram: @furiagg\nYouTube: FURIA\nTwitch: FURIAtv\nTikTok: @furiagg\nSite: furia.gg\nAgora digite o que deseja continuar sabendo sobre a Fúria:\nHistória\nJogadores\nAssistir\nComprar";
                case Intent.OndeAssistir:
                    session.State = "assistir_furia";
                    return "Acompanhe os jogos da Fúria nos canais:\nYouTube: https://www.youtube.com/@FURIAggCS\nTwitch: https://www.twitch.tv/furiatv\nAgora digite o que deseja continuar sabendo sobre a Fúria:\nHistória\nJogadores\nRedes sociais\nComprar";
                case Intent.Comprar:
                    session.State = "comprar_material";
                    return "Compre produtos oficiais da Fúria:\nhttps://www.furia.gg/\nAgora digite o que deseja continuar sabendo sobre a Fúria:\nHistória\nJogadores\nRedes sociais\nAssistir";
            }

            // Se não for nenhum intent conhecido, retorna o menu inicial objetivo
            if (session.State == "inicio")
            {
                session.State = "historia";
                session.LastTopic = "historia";
                return "E ai Furioso, sou o Pantera, chatbot oficial da Fúria E-sports. Aqui estão os assuntos que podemos falar sobre o nosso time de CS:\n" +
                       "História\n" +
                       "Jogadores\n" +
                       "Redes sociais\n" +
                       "Assistir\n" +
                       "Comprar\n" +
                       "Ou digite 'Valeu Pantera' para encerrar o chat.";
            }

            return "Não entendi, pode repetir a informação que deseja saber sobre a Fúria CS?";
        }

        private string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        private Intent DetectIntent(string input)
        {
            if (input.Contains("historia"))
                return Intent.Historia;

            if (input.Contains("titulo") || input.Contains("campeonato"))
                return Intent.Titulos;

            if (input.Contains("jogador"))
                return Intent.Jogadores;

            if (input.Contains("rede"))
                return Intent.RedesSociais;

            if (input.Contains("assist"))
                return Intent.OndeAssistir;

            if (input.Contains("comprar") || input.Contains("loja") || input.Contains("material"))
                return Intent.Comprar;

            if (input.Contains("valeu pantera") || input.Contains("encerrar") || input.Contains("tchau"))
                return Intent.Encerrar;

            return Intent.None;
        }
    }
}