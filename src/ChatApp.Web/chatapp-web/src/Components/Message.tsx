import type {GetChatMessagesResponse} from "../Pages/Chatroom.tsx";
/*import formatChatTimestamp from "../Helpers/FormatChatTimestamp.tsx";*/

export default function Message({index, username, message } : {index: number; username: string; message: GetChatMessagesResponse }) {
    const isUserMessage = message.senderUsername === username;

    const handleDelete = async (id: string) => {
        try {
            console.log("attempting to delete " + id);
            const deleteRes = await fetch(`https://localhost:7073/api/rooms/${roomSlug}/messages?messageId=${id}`, {
                method: 'DELETE',
                credentials: "include"
            });

            if (!deleteRes.ok) {
                // TODO: handle bad msgId provided
            }
        } catch {
            // TODO: handle
        }
    }

    return (
        <>
            <div key={index}
                 className={`chat ${isUserMessage ? "chat-end" : "chat-start"} my-2`}
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
                <div className="chat-header flex flex-row items-center">
                    {message.senderUsername}
                    {/*<time className="text-xs opacity-50">{formatChatTimestamp(message.createdAt)}</time>*/}

                    {isUserMessage && (
                        <div className="relative inline-block group">
                            {/* Button for message context menu */}
                            <button className="rounded px-2 py-1 hover:bg-base-300">
                                <svg className="h-5 w-5" fill="currentColor" viewBox="0 0 20 20">
                                    <path d="M6 10a2 2 0 11-4 0 2 2 0 014 0zm6 0a2 2 0 11-4 0 2 2 0 014 0zm6 0a2 2 0 11-4 0 2 2 0 014 0z" />
                                </svg>
                            </button>

                            {/* Menu */}
                            <div className="absolute right-0 bg-base-200 shadow rounded w-32 py-1
                              opacity-0 pointer-events-none
                              group-hover:opacity-100 group-hover:pointer-events-auto
                              transition z-50">
                                <button className="w-full text-left px-3 py-1 hover:bg-base-300">Edit</button>
                                <button onClick={() => handleDelete(message.id)} className="w-full text-left px-3 py-1 hover:bg-base-300">Delete</button>
                                <button className="w-full text-left px-3 py-1 hover:bg-base-300">React</button>
                            </div>
                        </div>
                    )}
                </div>
                <div className="chat-bubble">{message.content}</div>
                {/*<div className="chat-footer opacity-50">Delivered</div>*/}
            </div>
        </>
    )
}