import formatChatTimestamp from "../Helpers/FormatChatTimestamp.tsx";

export default function ChatTimeBreak({timestamp}: {timestamp: Date}) {

    return (
        <div className="flex justify-center my-4">
            <div className="px-3 py-1 text-xs text-gray-500 bg-gray-200 rounded-full select-none">
                {formatChatTimestamp(timestamp)}
            </div>
        </div>
    );
}
