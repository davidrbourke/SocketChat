﻿import { Component } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { HubConnection } from '@aspnet/signalr-client';

@Component({
    selector: 'chat',
    templateUrl: './chat.component.html'
})
export class ChatComponent {
    private _hubConnection: HubConnection;
    public async: any;
    message = '';
    messages: string[] = [];

    constructor() {
    }

    public sendMessage(): void {
        const data = `Sent: ${this.message}`;

        this._hubConnection.invoke('Send', data);
        this.messages.push(data);
    }

    ngOnInit() {
        this._hubConnection = new HubConnection('/chat');

        this._hubConnection.on('Send', (data: any) => {
            const received = `Received: ${data}`;
            this.messages.push(received);
        });

        this._hubConnection.on('Joined', (data: any) => {
            const received = `Joined: ${data}`;
            this.messages.push(received);
        });

        this._hubConnection.start()
            .then(() => {
                console.log('Hub connection started')
            })
            .catch(err => {
                console.log('Error while establishing connection')
            });
    }
}