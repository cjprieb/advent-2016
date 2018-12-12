open System.Text.RegularExpressions
#load "TestResources.fs"

let letters = List.append ['A'..'Z'] ['a'..'z'] 

let input = TestResources.ReadAllText("Day08.txt").Split(' ') |> Array.map int

let sample = [|2;3;0;3;10;11;12;1;1;0;1;99;2;1;1;2|]

// type Id = Id of char

type Header = {
    childCount:int
    metadataCount:int
}

type Node = {
    parent:int option
    Id:int
    header:Header
    children:int list
    metadata:int list
}

type State = ChildQuantity | MetadataQuantity | Metadata | Done

type Data = {
    nodeMap:Map<int,Node>
    parentId:int option
    currentId:int
    state:State    
}

module Node =
    let createNode id parent = {
        parent=parent
        Id=id
        header={
            childCount = 0
            metadataCount = 0
        }
        children=[]
        metadata=[]
    }

    let addChild id node =
        {
            parent=node.parent
            Id=node.Id
            header=node.header
            children=List.append node.children [id]
            metadata=node.metadata
        }

    let addMetadata value node =
        {
            parent=node.parent
            Id=node.Id
            header=node.header
            children=node.children
            metadata=List.append node.metadata [value]
        }

    let setMetadataCount value node =
        {
            parent=node.parent
            Id=node.Id
            header= {
                childCount = node.header.childCount
                metadataCount = value
            }
            children=node.children
            metadata=node.metadata
        }

    let setChildCount value node =
        {
            parent=node.parent
            Id=node.Id
            header= {
                childCount = value
                metadataCount = node.header.metadataCount
            }
            children=node.children
            metadata=node.metadata
        }

    let updateData state id parentId node map =
        // printfn "    Node %A: %A (%A); Metadata: %A (%A); Parent: %A -> %A|%A" 
        //     letters.[node.Id] 
        //     node.header.childCount 
        //     (node.children |> List.map (fun i -> letters.[i]))
        //     node.header.metadataCount 
        //     node.metadata
        //     node.parent
        //     state
        //     letters.[id] 
        {
            nodeMap=Map.add node.Id node map
            parentId=parentId
            currentId=id
            state=state
        }

let rec getParent (id:int) (nextAvailableId:int) (map:Map<int,Node>) =
    // printfn "  GetParent: %A" letters.[id]
    let node = map.[id]
    if node.header.metadataCount = List.length node.metadata then
        match node.parent with 
        | Some parentId -> getParent parentId nextAvailableId map
        | None -> (Done,id,node)
    else if node.header.childCount = List.length node.children then 
        // printfn "    Get metadata for %A" letters.[id]
        (Metadata,id,node)
    else 
        // printfn "    Get next child for %A (%A)" letters.[id] letters.[nextAvailableId]
        (ChildQuantity,nextAvailableId,node)

let processValue data value =
    let id = data.currentId
    // printfn "Processing %A %A for Node %A" data.state value letters.[id]
    if Map.containsKey id data.nodeMap then
        let node = data.nodeMap.[id]
        let (node',state',nextId,parentId,map') =
            match data.state,node.parent with 
            | ChildQuantity,_ -> 
                (node,MetadataQuantity,data.currentId,data.parentId,data.nodeMap)
            | MetadataQuantity,_ ->
                if node.header.childCount = 0 then
                    (Node.setMetadataCount value node),Metadata,data.currentId,data.parentId,data.nodeMap
                else 
                    let nextId = data.currentId+1
                    (Node.addChild nextId (Node.setMetadataCount value node)),ChildQuantity,nextId,Some node.Id,data.nodeMap
            | Metadata,Some parentId -> 
                if node.header.metadataCount = (List.length node.metadata + 1) then
                    let (nextState,nextId,parent) = getParent parentId (data.currentId + 1) data.nodeMap
                    let node' = Node.addMetadata value node
                    let map' = Map.add node.Id node' data.nodeMap
                    let node'' = if Map.containsKey nextId map' |> not then (Node.addChild nextId parent) else node'
                    node'',nextState,nextId,Some parent.Id,map'
                else 
                    (Node.addMetadata value node),Metadata,data.currentId,data.parentId,data.nodeMap
            | Metadata,_ -> 
                (Node.addMetadata value node),Metadata,data.currentId,data.parentId,data.nodeMap
            | Done,_ ->
                (node,MetadataQuantity,data.currentId,data.parentId,data.nodeMap)

        Node.updateData state' nextId parentId node' map'
    else
        let node = Node.setChildCount value (Node.createNode id data.parentId)
        Node.updateData MetadataQuantity data.currentId data.parentId node data.nodeMap

let build input = Array.fold processValue {nodeMap=Map.empty;parentId=None;currentId=0;state=ChildQuantity} input

let calculate map = Map.fold (fun sum _ value -> sum + (List.sum value.metadata)) 0 map

// 66406 is too high
// 49426 is correct
let solve1 input =
    let data = build input
    calculate data.nodeMap