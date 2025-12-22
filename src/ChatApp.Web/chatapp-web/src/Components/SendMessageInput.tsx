import {useState} from "react";

interface CreateChatMessageRequest {
    content: string;
    guestToken: string | null;
}

export default function SendMessageInput({roomSlug} : {roomSlug: string}) {
    const [messageDto, setMessageDto] = useState<CreateChatMessageRequest>({
        content: "",
        guestToken: null
    });

    const sendChatMessage = async (e: React.FormEvent) => {
        e.preventDefault();

        await fetch(`https://localhost:7073/api/rooms/${roomSlug}/messages`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            credentials: 'include',
            body: JSON.stringify(messageDto)
        });
    }

    return (
        <form onSubmit={sendChatMessage}>
            <input
                type="text"
                placeholder="Type here"
                className="input"
                value={messageDto?.content}
                name={"content"}
                onChange={(e) =>
                    setMessageDto(prev => ({
                        ...prev,
                        content: e.target.value,
                        guestToken: null
                    }))
                }
            />
            <button type="submit" hidden={true}></button>
        </form>
    )
}