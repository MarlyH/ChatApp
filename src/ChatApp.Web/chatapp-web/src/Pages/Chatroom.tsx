import {useParams} from "react-router";
import {useCallback, useContext, useEffect, useState} from "react";
import { HubConnectionBuilder } from "@microsoft/signalr";
import {UserContext} from "../UserContext.tsx";
import SendMessageInput from "../Components/SendMessageInput.tsx";
import {SERVER_URL} from "../Constants.tsx";
import ChatroomMessageList from "../Components/ChatroomMessageList.tsx";

interface GetRoomDetailsResponse {
    name: string;
}

export interface GetChatMessagesResponse {
    content: string;
    createdAt: string;
    senderUsername: string;
}

export default function Chatroom() {
    const {roomSlug} = useParams<{ roomSlug: string }>();
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const [roomDetails, setRoomDetails] = useState<GetRoomDetailsResponse>(
        {
            name: ""
        });
    const userContext = useContext(UserContext);
    const username = userContext?.user.username as string;
    const isLoggedIn = userContext?.user.isLoggedIn as boolean;
    const [messages, setMessages] = useState<GetChatMessagesResponse[]>([]);

    const fetchRoomDetails = useCallback(async () => {
        setIsLoading(true);

        try {
            const res = await fetch(`${SERVER_URL}/api/ChatRoom/${roomSlug}`);

            if (res.status === 404) {
                setError("Room Not Found");
            } else {
                const data = await res.json();
                setRoomDetails(data);
            }
        } catch {
            setError("Unable to load room.");
        } finally {
            setIsLoading(false);
        }
    }, [roomSlug]);

    const fetchPastMessages = useCallback(async () => {
        try {
            const pastMessagesRes = await fetch(`${SERVER_URL}/api/rooms/${roomSlug}/messages`);
            if (!pastMessagesRes.ok) {
                setError("Room Not Found");
            }

            const messages = await pastMessagesRes.json();
            setMessages(messages);
        } catch {
            setError("Unable to load past messages.");
        } finally {
            setIsLoading(false);
        }
    }, [roomSlug]);

    const checkJoinedRoom = useCallback(async () => {
        try {
            setIsLoading(true);

            // handle logged-in user
            if (isLoggedIn) {
                const joinRoomRes = await fetch(`${SERVER_URL}/api/rooms/${roomSlug}/join`, {
                    method: 'POST',
                    credentials: 'include',
                });

                if (!joinRoomRes.ok) {
                    const data = await joinRoomRes.json();
                    setError(data.message);
                    return false;
                }
            } else {
                // handle guest case, store guest token if newly-joined.
                const token: string | null = localStorage.getItem('GuestToken');

                const joinRoomRes = await fetch(`${SERVER_URL}/api/rooms/${roomSlug}/join-guest`, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify({
                        guestName: "Guest",
                        guestToken: token
                    })
                });
                const data = await joinRoomRes.json();

                if (!joinRoomRes.ok) {
                    setError(data.message);
                    return false;
                }

                localStorage.setItem('GuestToken', data.guestToken);
            }
        } catch {
            setError(`Unable to join room ${roomSlug}`);
            return false;
        } finally {
            setIsLoading(false);
        }

        console.log("User is joined to room.");
        return true;
    }, [isLoggedIn, roomSlug]); 

    useEffect(() => {
        // startup flow:
        // user navigates to chatroom
        // check user is joined, if not, automatically join them (resolve in one trip)
        // load room details + load messages
        const load = async () => {
            const isJoinSuccess: boolean = await checkJoinedRoom();
            if (!isJoinSuccess) {
                return;
            }

            // run in parallel:
            await Promise.all([
                fetchRoomDetails(),
                fetchPastMessages()
            ]);

            setError(null);
        };

        void load();
    }, [fetchRoomDetails, fetchPastMessages, checkJoinedRoom]);

    useEffect(() => {
        const hubConn = new HubConnectionBuilder()
            .withUrl(`${SERVER_URL}/chathub`)
            .withAutomaticReconnect()
            .build();

        // append the new message to the message list
        hubConn.on("MessageReceived", (message: GetChatMessagesResponse) => {
            setMessages(prev => [...prev, message]);
        });

        hubConn.on("UserJoined", message => {
            console.log(message);
        });

        // join the hub group associated with this chatroom.
        hubConn.start()
            .then(() => hubConn.invoke("JoinRoom", roomSlug, username ?? null, null))
            .then(() => console.log(roomSlug + " joined"))
            .catch(err => console.error("Hub start/join failed:", err));

        // rejoin the same room.
        hubConn.onreconnected(() => {
            hubConn.invoke("JoinRoom", roomSlug, username ?? null, null)
                .then(() => console.log(roomSlug + " rejoined"))
                .catch(err => console.error("Hub rejoin failed:", err));
        });

        return () => {
            hubConn.invoke("LeaveRoom", roomSlug)
                .finally(() => hubConn.stop());
        };
    }, [roomSlug, username]);

    return (
        <>
            {isLoading && (
                <p>Loading room...</p>
            )}

            {error && (
                <p>{error}</p>
            )}


            {!isLoading && (
                <div className="h-screen flex flex-col">

                    <h1 className="text-4xl font-bold tracking-tight text-base-content/95 m-4 shrink-0">
                        {roomDetails.name}
                    </h1>

                    <ChatroomMessageList messages={messages} />

                    <SendMessageInput roomSlug={roomSlug as string} />
                </div>
            )}
        </>
    )
}