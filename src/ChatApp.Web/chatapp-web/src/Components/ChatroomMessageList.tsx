import type {GetChatMessagesResponse} from "../Pages/Chatroom.tsx";
import {isTimeBreak} from "../Helpers/FormatChatTimestamp.tsx";
import ChatTimeBreak from "./ChatTimeBreak.tsx";
import Message from "./Message.tsx";
import {useContext, useEffect, useRef} from "react";
import {UserContext} from "../UserContext.tsx";

interface ChatroomMessageListProps {
    messages: GetChatMessagesResponse[];
    roomSlug: string;
}

export default function ChatroomMessageList({messages, roomSlug}: ChatroomMessageListProps)
{
    const bottomOfChatbox = useRef<HTMLDivElement | null>(null);
    const userContext = useContext(UserContext);
    const username = userContext?.user.username as string;

    // auto scroll to bottom of page when new message sent/received
    useEffect(() => {
        bottomOfChatbox.current?.scrollIntoView(true);
    }, [messages]);

    return (
        <div className="flex-1 overflow-y-auto w-full px-4">
            {(() => {
                let lastTimestamp: Date | null = null;
                return messages.map((message, index) => {
                    const msgDate = new Date(message.createdAt);
                    const showTime = isTimeBreak(lastTimestamp, msgDate);
                    // eslint-disable-next-line react-hooks/immutability
                    lastTimestamp = msgDate; // https://github.com/facebook/react/issues/31569

                    return (
                        <div key={index}>
                            {showTime && <ChatTimeBreak timestamp={msgDate} />}
                            <Message index={index} username={username} message={message} roomSlug={roomSlug} />
                        </div>
                    );
                });
            })()}
            <div ref={bottomOfChatbox} />
        </div>
    );
}