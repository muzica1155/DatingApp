//we have inteface in place for this particular fucntionality we will create a new service for our messages so 
// 
export interface Message {
    id: number;
    senderId: number;
    senderUsername: string;
    senderPhotoUrl: string;
    recipientId: number;
    recipientUsername: string;
    recipientPhotoUrl: string;
    content: string;
    dateRead: Date;
    messageSent: Date;
  }