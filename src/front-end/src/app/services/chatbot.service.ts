import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface UserMessageRequest {
  message: string;
  sessionId?: string | null;
}

export interface ChatResponseDto {
  response: string;
  sessionId: string;
}

@Injectable({ providedIn: 'root' })
export class ChatbotService {
  constructor(private http: HttpClient) {}

  sendMessage(request: UserMessageRequest): Observable<ChatResponseDto> {
    return this.http.post<ChatResponseDto>('/api/Chat/message', request);
  }
}