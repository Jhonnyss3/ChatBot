import { Component, ViewChild, ElementRef, AfterViewChecked } from '@angular/core';
import { ChatbotService, UserMessageRequest, ChatResponseDto } from './services/chatbot.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, FormsModule], 
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements AfterViewChecked {
  mensagens: { texto: string, autor: 'user' | 'bot' }[] = [];
  textoUsuario: string = '';
  sessionId: string | null = null;

  @ViewChild('chatBody') chatBody!: ElementRef<HTMLDivElement>;

  constructor(private chatbotService: ChatbotService) {}

  enviarMensagem() {
    if (!this.textoUsuario.trim()) return;

    this.mensagens.push({ texto: this.textoUsuario, autor: 'user' });

    const req: UserMessageRequest = {
      message: this.textoUsuario,
      sessionId: this.sessionId
    };

    this.textoUsuario = '';

    this.chatbotService.sendMessage(req).subscribe((res: ChatResponseDto) => {
      this.sessionId = res.sessionId;
      this.mensagens.push({ texto: res.response, autor: 'bot' });
    });
  }

  ngAfterViewChecked() {
    this.scrollToBottom();
  }

  scrollToBottom() {
    try {
      this.chatBody.nativeElement.scrollTop = this.chatBody.nativeElement.scrollHeight;
    } catch (err) {}
  }

  iniciarConversaPadrao() {
    // Limpa o chat e a sessão, começando do zero
    this.mensagens = [];
    this.sessionId = null;
  
    // Mensagem padrão
    const mensagemPadrao = 'Olá';
  
    // Adiciona a mensagem do usuário
    this.mensagens.push({ texto: mensagemPadrao, autor: 'user' });
  
    const req: UserMessageRequest = {
      message: mensagemPadrao,
      sessionId: this.sessionId
    };
  
    this.textoUsuario = ''; // Limpa o input, se necessário
  
    this.chatbotService.sendMessage(req).subscribe((res: ChatResponseDto) => {
      this.sessionId = res.sessionId;
      this.mensagens.push({ texto: res.response, autor: 'bot' });
    });
  }

  // Lista de opções a destacar
  OPCOES_CHAT = [
    'Nossa história',
    'Nossos jogadores',
    'Nossas redes sociais',
    'Onde assisto a Furia?',
    'Onde compro materiais oficiais da Fúria?',
    'Valeu Pantera'
  ];

formatarMensagem(texto: string): string {
  let textoFormatado = texto;
  this.OPCOES_CHAT.forEach(opcao => {
    // Regex para destacar a opção, escapando caracteres especiais
    const regex = new RegExp(opcao.replace(/[.*+?^${}()|[\]\\]/g, '\\$&'), 'g');
    textoFormatado = textoFormatado.replace(
      regex,
      `<span class="chat-option">${opcao}</span>`
    );
  });
  // Opcional: tratar quebras de linha
  return textoFormatado.replace(/\n/g, '<br>');
}
}