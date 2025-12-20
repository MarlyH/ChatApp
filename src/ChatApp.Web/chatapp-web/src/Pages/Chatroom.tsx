import {useParams} from "react-router";
import {useCallback, useEffect, useState} from "react";
import { HubConnectionBuilder } from "@microsoft/signalr";

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
            .withAutomaticReconnect() // does not reconnect to the group
            .build();

        hubConn.on("MessageReceived", message => {
            console.log(message);
        });

        hubConn.start()
            .then(() => hubConn.invoke("JoinRoom", roomSlug))
            .then(() => console.log(roomSlug + " joined"))
            .catch(err => console.error("Hub start/join failed:", err));
    }, [roomSlug]);

    return (

        <>
            {isLoading && (
                <p>Loading room...</p>
            )}

            {error && (
                <p>{error}</p>
            )}

            {!isLoading && (
                <h1>{roomDetails.name}</h1>
            )}
        </>
    )
}