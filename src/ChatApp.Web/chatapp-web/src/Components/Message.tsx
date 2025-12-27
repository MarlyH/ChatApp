import type {GetChatMessagesResponse} from "../Pages/Chatroom.tsx";
import {useState} from "react";
/*import formatChatTimestamp from "../Helpers/FormatChatTimestamp.tsx";*/

export default function Message({index, username, message } : {index: number; username: string; message: GetChatMessagesResponse }) {
    const [menuOpen, setMenuOpen] = useState(false);

    return (
        <>
            <div key={index}
                 className={`chat ${message.senderUsername === username ? "chat-end" : "chat-start"}`}
            >
                <div className="chat-image avatar">
                    <div className="w-10 rounded-full">
                        <img
                            alt="Tailwind CSS chat bubble component"
                            src={`${message.senderUsername !== username
                                ? "https://img.daisyui.com/images/profile/demo/kenobee@192.webp"
                                : "https://img.daisyui.com/images/profile/demo/anakeen@192.webp"}`}
                        />
                    </div>
                </div>
                <div className="chat-header">
                    {message.senderUsername}
                    {/*<time className="text-xs opacity-50">{formatChatTimestamp(message.createdAt)}</time>*/}

                    {/* Button for message context menu */}
                    <button
                        className="rounded hover:bg-base-300 px-2 pt-1"
                        onClick={() => setMenuOpen(!menuOpen)}
                    >
                        <svg
                            xmlns="http://www.w3.org/2000/svg"
                            className="h-5 w-5"
                            fill="currentColor"
                            viewBox="0 0 20 20"
                        >
                            <path d="M6 10a2 2 0 11-4 0 2 2 0 014 0zm6 0a2 2 0 11-4 0 2 2 0 014 0zm6 0a2 2 0 11-4 0 2 2 0 014 0z" />
                        </svg>
                    </button>

                    {/* Menu */}
                    {menuOpen && (
                        <div className="bg-base-200 shadow rounded w-32 py-1 z-10">
                            <button className="w-full text-left px-3 py-1 hover:bg-base-300">Edit</button>
                            <button className="w-full text-left px-3 py-1 hover:bg-base-300">Delete</button>
                            <button className="w-full text-left px-3 py-1 hover:bg-base-300">React</button>
                        </div>
                    )}
                </div>
                <div className="chat-bubble">{message.content}</div>
                <div className="chat-footer opacity-50">Delivered</div>
            </div>
        </>
    )
}