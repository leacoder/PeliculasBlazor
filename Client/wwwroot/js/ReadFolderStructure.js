async function readData()
{
    const out = [];
    const outFolder = [];
    const dirHandle = await showDirectoryPicker();
    out[0] = { "id": 0, "type": "directory", "hasChildren": true, "folderName": dirHandle.name, "name": dirHandle.name, "content": null, "children": [] };
await handleDirectoryEntry(dirHandle, out[0].children);
await handleOnlyDirectoryEntry(dirHandle, outFolder);

return { 'FullTree': out, 'FolderTree': outFolder };
}

async function handleDirectoryEntry(dirHandle, out)
{
    var index = 0;
    for await (const entry of dirHandle.values()) {
    if (entry.kind === "file")
    {
        const file = await entry.getFile();
        const fileContentObject = await file.stream().getReader().read();
            out[index] = { "id": index, "type": "file", "hasChildren": false, "name": entry.name, "content": fileContentObject.value, "children": [] };
        index++;
    }
    if (entry.kind === "directory")
    {
        const newOut = out[index] = { "id": index, "hasChildren": true, "type": "directory", "folderName": entry.name, "name": entry.name, "content": null, "children": [], "icon": "folder" };
        index++;
        await handleDirectoryEntry(entry, newOut.children);
    }
}
}

async function handleOnlyDirectoryEntry(dirHandle, outFolder)
{
    var index = 0;

    for await (const entry of dirHandle.values()) {
    if (entry.kind === "directory")
    {
        const newOut = outFolder[index] = { "id": index, "type": "directory", "name": entry.name, "content": null, "children": [] };
        index++;
        await handleOnlyDirectoryEntry(entry, newOut.children);
    }
}
}