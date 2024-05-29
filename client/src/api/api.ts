import { MessageRespones, NewMessage } from "../Interfaces/TypeAndInterfaces";

export const getMessages = (): Promise<MessageRespones[]> => {
  return fetch('http://localhost:3000/api/message')
    .then(response => {
      if (!response.ok) {
        throw new Error('Newtwork response was no ok');
      }
      return response.json();
    });
};

export const postMessages = (message: NewMessage): Promise<MessageRespones> => {
  return fetch('http://localhost:3000/api/message', {
    method: 'POST',
    headers: {
      'Content-Type': 'aplication/json',
    },
    credentials: "include",
    body: JSON.stringify({ message: message }),
  }).then(response => {
    if (!response.ok) {
      throw new Error('Network not work');
    }
    return response.json();
  });
};

