import React, {useState} from "react";
import {SERVER_URL} from "../Constants.tsx";

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

        await fetch(`${SERVER_URL}/api/rooms/${roomSlug}/messages`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            credentials: 'include',
            body: JSON.stringify(messageDto)
        });

        // clear message input after sending
        setMessageDto(prev => ({
            ...prev,
            content: ""
        }))
    }

    return (
        <div className="w-full shrink-0">
            <form onSubmit={sendChatMessage}>
                <input
                    type="text"
                    placeholder="Type here"
                    className="input w-full"
                    value={messageDto?.content}
                    name={"content"}
                    onChange={(e) =>
                        setMessageDto(prev => ({
                            ...prev,
                            content: e.target.value
                        }))
                    }
                />
                <button type="submit" hidden={true}></button>
            </form>
        </div>
    )
}