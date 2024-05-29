import { ChangeEvent, FormEvent, useEffect, useState } from "react";
import { getMessages, postMessages } from "../api/api";
import { MessageRespones, NewMessage } from "../Interfaces/TypeAndInterfaces";

function Message() {
    const [messages, setMessages] = useState<MessageRespones[]>([]);
    const [newMessage, setNewMessage] = useState<string>('');

    useEffect(() => {
        getMessages()
            .then((data: MessageRespones[]) => setMessages(data))
            .catch((error: Error) => console.error('Erro fetching messages', error))
    }, []);
    const handInput = (e: ChangeEvent<HTMLInputElement>) => { setNewMessage(e.target.value) }

    const handleSubmit = (e: FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        if (newMessage) {
            const messageSending: NewMessage = { text: newMessage }
            postMessages(messageSending)
                .then((response: MessageRespones) => {
                    setMessages((currentMessages) => [...currentMessages, response]);
                    setNewMessage('');
                })
                .catch((error: Error) => console.error('error posting message', error));
        }
    }

    return (
        <>
            <section className="Ms">
                <h1>Messages</h1>
                <form onSubmit={handleSubmit}>
                    <input
                        type="text"
                        placeholder="Write your text over here"
                        value={newMessage}
                        onChange={handInput} />
                    <button type="submit"> Send</button>
                </form>
                <ul>
                    {messages.map((message) => (
                        <li key={message.id}>{message.text}</li>
                    ))}
                </ul>
            </section>
        </>
    )
}

export default Message;