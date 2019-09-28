export interface IChatPreview {
    id: string;
    type: ChatPreviewType;
    name: string;
    lastMessage: string;
    newMessagesCount: number;
}

export enum ChatPreviewType {
    GroupChat = 0,
    SingleChat = 1
}