/// <reference name="MicrosoftAjax.js"/>

// Much of this file is based on ndp\fx\src\xsp\System\Web\ExtensionsScript\Sys\WebForms\PageRequestManager.js.

Type.registerNamespace("Sys.Mvc");

Sys.Mvc.InsertionMode = function() {
    throw Error.notImplemented();
}

Sys.Mvc.InsertionMode.prototype = {
    Replace: 0,
    InsertBefore: 1,
    InsertAfter: 2
}

Sys.Mvc.InsertionMode.registerEnum('Sys.Mvc.InsertionMode');

Sys.Mvc._onComplete = function(executor, ajaxOptions) {

    var targetElement = $get(ajaxOptions.updateTargetId);
    var insertionMode = ajaxOptions.insertionMode;
    var request = executor.get_webRequest();

    // Insert the results into the target element
    if (targetElement) {
        switch (insertionMode) {
            case Sys.Mvc.InsertionMode.Replace:
                targetElement.innerHTML = executor.get_responseData();
                break;
            case Sys.Mvc.InsertionMode.InsertBefore:
                targetElement.innerHTML = executor.get_responseData() + targetElement.innerHTML;
                break;
            case Sys.Mvc.InsertionMode.InsertAfter:
                targetElement.innerHTML = targetElement.innerHTML + executor.get_responseData();
                break;
        }
    }

    var statusCode = executor.get_statusCode();
    if (statusCode >= 200 && statusCode < 300) {
        if (ajaxOptions.onSuccess) {
            ajaxOptions.onSuccess(request);
        }
    }
    else {
        if (ajaxOptions.onFailure) {
            ajaxOptions.onFailure(request);
        }
    }
}

Sys.Mvc.AsyncForm = function() {
}

Sys.Mvc.AsyncForm.prototype = {
}

Sys.Mvc.AsyncForm.serializeForm = function(form) {
    var formElements = form.elements;
    var formBody = new Sys.StringBuilder();

    var count = formElements.length;
    for (var i = 0; i < count; i++) {
        var element = formElements[i];
        var name = element.name;
        if (!name || !name.length) {
            continue;
        }

        // DevDiv Bugs 146697: tagName needs to be case insensitive to work with xhtml content type
        var tagName = element.tagName.toUpperCase();

        if (tagName === 'INPUT') {
            var type = element.type;
            if ((type === 'text') ||
                (type === 'password') ||
                (type === 'hidden') ||
                (((type === 'checkbox') || (type === 'radio')) && element.checked)) {
                // DevDiv Bugs 109456: Encode the name as well as the value
                formBody.append(encodeURIComponent(name));
                formBody.append('=');
                formBody.append(encodeURIComponent(element.value));
                formBody.append('&');
            }
        }
        else if (tagName === 'SELECT') {
            var optionCount = element.options.length;
            for (var j = 0; j < optionCount; j++) {
                var option = element.options[j];
                if (option.selected) {
                    // DevDiv Bugs 109456: Encode the name as well as the value
                    formBody.append(encodeURIComponent(name));
                    formBody.append('=');
                    formBody.append(encodeURIComponent(option.value));
                    formBody.append('&');
                }
            }
        }
        else if (tagName === 'TEXTAREA') {
            // DevDiv Bugs 109456: Encode the name as well as the value
            formBody.append(encodeURIComponent(name));
            formBody.append('=');
            formBody.append(encodeURIComponent(element.value));
            formBody.append('&');
        }
    }

    return formBody.toString();
}

Sys.Mvc.AsyncForm.handleSubmit = function(form, ajaxOptions) {
    var body = Sys.Mvc.AsyncForm.serializeForm(form);
    body += "__MVCAJAX=true";

    // TODO: need to pull target method from form
    var action = form.action;
    var method = "POST";

    // Make the request
    var request = new Sys.Net.WebRequest();
    request.set_url(action);
    request.set_httpVerb(method);
    request.set_body(body);
    request.add_completed(Sys.Mvc.AsyncForm._createDelegate(form, ajaxOptions));

    if (ajaxOptions.onBegin) {
        ajaxOptions.onBegin(request);
    }
    request.invoke();
}

Sys.Mvc.AsyncForm._createDelegate = function(form, ajaxOptions) {
    return function(executor, eventArgs) {
        form.reset();
        Sys.Mvc._onComplete(executor, ajaxOptions);
    }
}

Sys.Mvc.AsyncForm.registerClass('Sys.Mvc.AsyncForm');

Sys.Mvc.AsyncHyperlink = function() {
}

Sys.Mvc.AsyncHyperlink.prototype = {
}

Sys.Mvc.AsyncHyperlink.handleClick = function(anchor, ajaxOptions) {
    var targetUrl = anchor.href;
    var method = "POST";
    var body = "__MVCAJAX=true";

    // Make the request
    var request = new Sys.Net.WebRequest();
    request.set_url(targetUrl);
    request.set_httpVerb(method);
    request.set_body(body);
    request.add_completed(Sys.Mvc.AsyncHyperlink._createDelegate(ajaxOptions));

    if (ajaxOptions.onBegin) {
        ajaxOptions.onBegin(request);
    }
    request.invoke();
}

Sys.Mvc.AsyncHyperlink._createDelegate = function(ajaxOptions) {
    return function(executor, eventArgs) {
        Sys.Mvc._onComplete(executor, ajaxOptions);
    }
}

Sys.Mvc.AsyncHyperlink.registerClass('Sys.Mvc.AsyncHyperlink');
