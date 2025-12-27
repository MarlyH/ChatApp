import type {GetChatMessagesResponse} from "../Pages/Chatroom.tsx";
/*import formatChatTimestamp from "../Helpers/FormatChatTimestamp.tsx";*/

export default function Message({index, username, message } : {index: number; username: string; message: GetChatMessagesResponse }) {
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
                </div>
                <div className="chat-bubble">{message.content}</div>
                <div className="chat-footer opacity-50">Delivered</div>
            </div>
        </>
    )
}