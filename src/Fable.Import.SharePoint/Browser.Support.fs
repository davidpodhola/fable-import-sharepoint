﻿module Browser.Support

open Fable.Import.SharePoint.SP
open Fable.Core
open Fable.Import.Browser
open Fable.Core.JsInterop

// http://www.fssnip.net/9l
open Microsoft.FSharp.Reflection

let onDocumentReady (callback:unit->unit) : unit =
    document.onreadystatechange <- fun _ -> 
        if document.readyState = "complete" then
            callback ()
        null

let windowParentLocation =
    try 
        Some(window.parent.location.href)
    with
    | _ -> None

let getIndexOfUrlPart (part : string) : int =
    window.location.href.IndexOf(part)

let locationHasPart (part : string) =
    getIndexOfUrlPart part > -1

let getCurrentUrl () =
    window.location.href

let parentHasPart (part : string) =
    let parent = windowParentLocation
    match parent with
    | Some(loc) -> loc.IndexOf(part) > -1
    | _ -> false

[<Emit("alert($0)")>]
let alert (x: string) : unit = jsNative

[<Emit("console.log($0)")>]
let log (message:string) : unit = jsNative

[<Emit("console.log($0)")>]
let logO (value:obj) : unit = jsNative

[<Emit("jQuery($0)")>]
let el (cssSelector:string) = jsNative

let elH (elementId:string) = el("#"+elementId)

let setElementHValue (elementId:string) (text:string) =
    (elH (elementId))?``val``( text ) |> ignore

let setElementValue (selector:string) (text:string) =
    (el (selector))?``val``( text ) |> ignore

let checkRadio (elementId:string) =
    (elH (elementId))?prop("checked",true) |> ignore

[<Emit("jQuery()")>]
let jQ () = jsNative

type ajaxParameters = {
    beforeSend : obj -> unit
    ``type`` : string
    url : string
    data : string
    dataType : string
    success : string -> unit
}

[<Emit("jQuery.ajax($0)")>]
let ajax (parameter:ajaxParameters) = jsNative


let postJSON url data callback =
    ajax(
        let beforeSend (xhrObj) =
            xhrObj?setRequestHeader("Content-Type","application/json") |> ignore
            xhrObj?setRequestHeader("Accept","application/json")  |> ignore
    
        {
            beforeSend = beforeSend
            ``type`` = "POST"
            url = url     
            data = data
            dataType = "json"
            success = callback
        }
    )

let showAll el = 
    el?find("*")?show() |> ignore
    el?show() |> ignore

let change el func = el?change(func)
let submit el func = el?submit(func)
let attr el value:string = el?attr(value).ToString()
let hide el = el?hide()
let show el = el?show()
let append el (value: obj) = el?append(value)
let parent el = el?parent()
let remove el = el?remove()
[<Emit("setTimeout($0,$1)")>]
let setTimeout (callback:unit->unit) (miliseconds) = jsNative
let prop el (value : string*'A) = 
                  let name, v = value
                  el?prop(name,v)              
