import React, {useState} from "react";
import {SERVER_URL} from "../Constants.tsx";

export default function SendMessageInput({roomSlug} : {roomSlug: string}) {
    const [content, setContent] = useState("");

    const sendChatMessage = async (e: React.FormEvent) => {
        e.preventDefault();

        const payload = {
            guestToken: localStorage.getItem('GuestToken'),
            content
        }

        console.log(payload);

        await fetch(`${SERVER_URL}/api/rooms/${roomSlug}/messages`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            credentials: 'include',
            body: JSON.stringify(payload)
        });

        // clear message input after sending
        setContent("");
    }

    return (
        <div className="w-full shrink-0">
            <form onSubmit={sendChatMessage}>
                <input
                    type="text"
                    placeholder="Type here"
                    className="input w-full"
                    value={content}
                    name={"content"}
                    onChange={(e) =>
                        setContent(e.target.value)
                    }
                />
                <button type="submit" hidden={true}></button>
            </form>
        </div>
    )
}