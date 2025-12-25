import {useEffect, useState} from "react";
import {Link} from "react-router";
import {SERVER_URL} from "../Constants.tsx";

interface GetRoomsResponse {
    id: string;
    name: string;
    slug: string;
}

export default function ChatroomList() {
    const [isLoading, setIsLoading] = useState(true);
    const [error, setError] = useState<string>("");
    const [chatrooms, setChatrooms] = useState<GetRoomsResponse[]>([]);

    const fetchChatrooms = async () => {
        setIsLoading(true);

        try {
            const res = await fetch(`${SERVER_URL}/api/ChatRoom`);
            const data = await res.json();
            const mappedData: GetRoomsResponse[] = data.map((chatroom: GetRoomsResponse) => ({
                id: chatroom.id,
                name: chatroom.name,
                slug: chatroom.slug,
            }));

            setChatrooms(mappedData);
        } catch {
            setError("Unable to fetch the chatroom list.");
        } finally {
            setIsLoading(false);
        }
    }
    useEffect(() => {
        void fetchChatrooms();
    }, []);

    return (
        <>
            <ul className="list bg-base-100 rounded-box shadow-md">

                <li className="p-4 pb-2 text-xs opacity-60 tracking-wide">Public Chatrooms</li>
                {isLoading && (
                    <p>Loading chatrooms...</p>
                )}

                {error && (
                    <p>{error}</p>
                )}

                {chatrooms.map((chatroom: GetRoomsResponse) => (
                    <Link to={`/room/${chatroom.slug}`}>
                        <li className="list-row" key={chatroom.id}>
                            <div><img className="size-10 rounded-box" src="https://img.daisyui.com/images/profile/demo/1@94.webp"/></div>
                            <div>
                                <div>{chatroom.name}</div>
                                <div className="text-xs uppercase font-semibold opacity-60">{chatroom.slug}</div>
                            </div>
                        </li>
                    </Link>
                ))}
            </ul>
        </>
    )
}