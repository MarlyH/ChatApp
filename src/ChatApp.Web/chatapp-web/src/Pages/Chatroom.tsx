import {useParams} from "react-router";
import {useCallback, useContext, useEffect, useState} from "react";
import { HubConnectionBuilder } from "@microsoft/signalr";
import {UserContext} from "../UserContext.tsx";
import SendMessageInput from "../Components/SendMessageInput.tsx";

interface GetRoomDetailsResponse {
    name: string;
}

export default function Chatroom() {
    const {roomSlug} = useParams<{ roomSlug: string }>();

    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState<string>("");
    const [roomDetails, setRoomDetails] = useState<GetRoomDetailsResponse>(
        {
            name: ""
        });
    const userContext = useContext(UserContext);
    const username = userContext?.user.username;

    const fetchRoomDetails = useCallback(async () => {
        setIsLoading(true);

        try {
            const res = await fetch(`https://localhost:7073/api/ChatRoom/${roomSlug}`);

            if (res.status === 404) {
                setError("Room Not Found");
            } else {
                const data = await res.json();
                setRoomDetails(data)
            }
        } catch {
            setError("Unable to load room.");
        } finally {
            setIsLoading(false);
        }
    }, [roomSlug]);

    useEffect(() => {
        void fetchRoomDetails();
        }, [fetchRoomDetails]);

    useEffect(() => {
        const hubConn = new HubConnectionBuilder()
            .withUrl("https://localhost:7073/chathub")
            .withAutomaticReconnect()
            .build();

        hubConn.on("MessageReceived", message => {
            console.log(message);
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
                <>
                    <h1>{roomDetails.name}</h1>
                    <SendMessageInput roomSlug={roomSlug as string} />
                </>
            )}
        </>
    )
}