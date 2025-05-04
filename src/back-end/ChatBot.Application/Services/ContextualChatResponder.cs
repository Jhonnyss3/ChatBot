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

            
            if (intent == Intent.Encerrar)
            {
                session.State = "encerrado";
                await _sessionService.RemoveSessionAsync(session.SessionId);
                return "Eu que agradeço e espero que você tenha encontrado tudo que precisa! GO FÚRIA!";
            }

            
            switch (intent)
            {
                case Intent.Historia:
                    session.State = "titulos";
                    return "A FURIA Esports foi fundada em 2017 com a missão de se tornar uma potência no cenário de CS:GO. Desde o começo, a equipe se destacou por sua energia única e um estilo de jogo agressivo, ganhando rapidamente espaço no Brasil e no mundo.\r\nEm 2019, o time brilhou ao derrotar grandes nomes, como a Astralis, e se consolidou como uma das principais equipes do Brasil. A chegada de FalleN em 2023 trouxe ainda mais poder ao nosso elenco, aumentando a sede por títulos e a ambição de continuar sendo referência no cenário internacional.\r\nHoje, a FURIA segue firme, representando o Brasil no mundo do CS:GO e mostrando sua garra em cada torneio.\n\nQuer saber mais sobre nossos títulos? Digite 'Títulos'! &#127941";
                case Intent.Titulos:
                    session.State = "titulos_lista";
                    return "A FURIA Esports conquistou muitos momentos marcantes e inesquecíveis no CS:GO, desde sua ascensão ao topo do cenário brasileiro até as vitórias em grandes torneios internacionais. Em 2019, o time já mostrava a que veio, vencendo a DreamHack Masters Dallas e surpreendendo o mundo ao derrotar gigantes como a Astralis.\r\n\r\nEm 2020, a FURIA não parou e garantiu o título da ESL Pro League Season 12: North America, carimbando seu nome entre os melhores do mundo. Em 2021, o time continuou em alta, levantando a taça da IEM New York e conquistando a BLAST Premier: Spring American Finals.\r\n\r\nEsses títulos não só solidificaram a FURIA como uma potência no CS, mas também a colocaram como referência para outras equipes brasileiras e internacionais. Com uma trajetória de consistência e paixão, a FURIA segue sendo uma das maiores organizações do CS:GO, com seus olhos sempre voltados para novos desafios e conquistas.\n\nAgora, escolha o que você quer saber mais sobre a FURIA:\n&#128073 Jogadores\n&#128241 Redes sociais\n&#127909 Assistir\n&#128722 Comprar";
                case Intent.Jogadores:
                    session.State = "jogadores";
                    return "A FURIA Esports segue forte no cenário internacional de CS:GO com uma line-up renovada, destacando-se como uma das principais equipes brasileiras. A formação atual é composta por jogadores experientes e novos talentos:\n&#127919Gabriel 'FalleN' Toledo (@fallen): AWPer / IGL\n&#129521Kaike 'KSCERATO' Cerato (@kscerato): Rifler\n&#128165Yuri 'yuurih' Santos (@yuurihfps): Rifler\n&#127919Danil 'molodoy' Golubenko: Sniper\n&#128165Mareks 'YEKINDAR' Gaļinskis(@yek1ndar): Rifler(Stand-In)\n&#129504Coach: Sidnei 'sidde' (@siddecs)\n\nAgora, escolha o que você quer saber mais sobre a FURIA:\n&#128214 História\n&#128241 Redes sociais\n&#127909 Assistir\n&#128722 Comprar";
                case Intent.RedesSociais:
                    session.State = "redes_sociais";
                    return "Você vai encontrar tudo sobre a Fúria aqui:\n&#128036 Twitter: @FURIA\n&#128248 Instagram: @furiagg\n&#9193 YouTube: FURIA\n&#127918 Twitch: FURIAtv\n&#127925 TikTok: @furiagg\n&#127760 Site: furia.gg\nAgora, escolha o que você quer saber mais sobre a FURIA:\n&#128214 História\n&#128241 Redes sociais\n&#127909 Assistir\n&#128722 Comprar";
                case Intent.OndeAssistir:
                    session.State = "assistir_furia";
                    return "&#127909 Quer ver a FURIA em ação? Acompanhe nossos jogos ao vivo e conteúdos exclusivos pelos canais oficiais:\n&#9193 YouTube: https://www.youtube.com/@FURIAggCS\n&#127918 Twitch: https://www.twitch.tv/furiatv\nAgora digite o que deseja continuar sabendo sobre a Fúria:\n&#128214 História\n&#128073 Jogadores\n&#127909 Redes sociais\n&#128722 Comprar";
                case Intent.Comprar:
                    session.State = "comprar_material";
                    return "&#128722 Quer vestir a camisa da FURIA?\nGaranta seus produtos oficiais e mostre sua torcida com orgulho! &#128165\n\n&#128073 https://www.furia.gg/\n\nAgora digite o que deseja continuar sabendo sobre a Fúria:\n&#128214 História\n&#128073 Jogadores\n&#127909 Redes sociais\n&#127909 Assistir";
            }

            
            if (session.State == "inicio")
            {
                session.State = "historia";
                session.LastTopic = "historia";
                return "&#128075 E ai Furioso! Sou o Pantera, chatbot oficial da Fúria E-sports. Aqui estão os assuntos que podemos falar sobre o nosso time de CS:\n" +
                       "&#128214 História\n" +
                       "&#128073 Jogadores\n" +
                       "&#127909 Redes sociais\n" +
                       "&#127909 Assistir\n" +
                       "&#128722 Comprar\n" +
                       "Ou, se quiser encerrar o papo, digite 'Valeu Pantera'. &#128526";
            }

            return "&#129300 Não entendi muito bem...\nPode repetir a informação que deseja saber sobre a Fúria CS? Você pode escolher entre:\n&#128214 História\n&#128073 Jogadores\n&#127909 Redes sociais\n&#127909 Assistir\n&#128722 Comprar\nOu digitar 'Valeu Pantera' para encerrar o chat. &#128516";
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