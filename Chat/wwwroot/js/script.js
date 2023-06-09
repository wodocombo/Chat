/*SignalR*/
const hubConnection = new signalR.HubConnectionBuilder()
    .withUrl("/chat")
    .build();

hubConnection.start()
    .then(function () {
        console.log("ura");
    })
    .catch(function (err) {
        return console.error(err.toString());
    });

/*SignalR*/


/*Кнопка регистации 2*/
const login_button = document.getElementById('login');
const modal_container = document.getElementById('modal-container');

if (login_button != null) {
    LoginAddEvent();
}
function LoginAddEvent() {
    login_button.addEventListener('click', () => {
        fetch("/Identity/LoginForm")
            .then(response => {
                if (response.ok) {
                    return response.text();
                } else {
                    return response.text().then(error => {
                        throw new Error(error);
                    })
                }
            })
            .then(text => {
                modal_container.classList.add('show');
                modal_container.innerHTML = text;
                switchingLoginForm();
            })
            .catch(error => {
                alert(error.message);
            })
    })
}
/*Кнопка регистации 2*/


/*Кнопка загрузки Логин формы*/
function switchingLoginForm() {

    var form_login = document.getElementById('modal-login-form');
    var form_registration = document.getElementById('modal-registration-form');

    var button_load_login_form = document.getElementById('load-login-form');
    var button_load_registration_form = document.getElementById('load-registration-form');


    button_load_login_form.addEventListener('click', () => {
        form_registration.classList.remove('active');
        form_login.classList.add('active');
    })

    button_load_registration_form.addEventListener('click', () => {
        form_login.classList.remove('active');
        form_registration.classList.add('active');
    })

    setTimeout(modalEventAdd, 0.5);

    function modalEventAdd() {
        document.addEventListener('click', modalClickOut);
    }

    function modalClickOut(event) {

        const modal = document.getElementById('modal');
        const isClickInside = modal.contains(event.target);

        if (!isClickInside) {
            modal_container.classList.remove('show');
            modal_container.innerHTML = "";
            document.removeEventListener('click', modalClickOut);

        }
    }
}
/*Кнопка загрузки Логин формы*/


/*Логин пользователя*/
function Login(event) {
    event.stopPropagation();
    event.preventDefault();

    loginForm = document.getElementById('form-login');

    const loginFormData = new FormData(loginForm);

    fetch("/Login", {
        method: 'POST',
        body: loginFormData
    })
        .then(response => {
            if (response.ok) {
                location.reload();
            } else {
                return response.text().then(error => {
                    throw new Error(error);
                })
            }
        })
        .catch(error => {
            alert(error.message);
        })
}
/*Логин пользователя*/


/*Регистрация пользователя*/
function Registration(event) {
    event.stopPropagation();
    event.preventDefault();

    registrationForm = document.getElementById('form-registration');

    const registrationFormData = new FormData(registrationForm);

    fetch("/Registration", {
        method: 'POST',
        body: registrationFormData
    })
        .then(response => {
            if (response.ok) {
                location.reload();
            } else {
                return response.text().then(error => {
                    throw new Error(error);
                });
            }
        })
        .catch(error => {
            alert(error.message);
        })
}
/*Регистрация пользователя*/


/*Загрузка профиля пользователя (добавление Event)*/
const user_button = document.getElementById('user');

if (user_button != null) {
    UserAddEvent();
}
/*Загрузка профиля пользователя (добавление Event)*/


/*Загрузка профиля пользователя*/
function UserAddEvent() {
    user_button.addEventListener('click', () => {
        const modal_container = document.getElementById('modal-container');

        fetch("/User/Profile")
            .then(response => {
                if (response.ok) {
                    return response.text();
                }
                else {
                    return response.text().then(error => {
                        throw Error(error);
                    })
                }
            })
            .then(text => {
                modal_container.classList.add('show');
                modal_container.innerHTML = text;
                LoadUserProfileEvents();
            })
            .catch(error => {
                alert(error.message);
            });
    });
}
/*Загрузка профиля пользователя*/


/*Загрузка всех ивентов профиля пользователя*/
function LoadUserProfileEvents() {
    HideUserProfile();
    ExitFromUserProfile();
}
/*Загрузка всех ивентов профиля пользователя*/


/*Скрытие профиля пользователя*/
function HideUserProfile() {
    modalEventAdd();
    function modalEventAdd() {
        document.addEventListener('click', modalClickOut);
    }

    function modalClickOut(event) {

        const modal = document.getElementById('modal');
        const isClickInside = modal.contains(event.target);

        if (!isClickInside) {
            modal_container.classList.remove('show');
            modal_container.innerHTML = "";
            document.removeEventListener('click', modalClickOut);
        }
    }
}
/*Скрытие профиля пользователя*/


/*Выход из профиля пользователя*/
function ExitFromUserProfile() {
    const exitFromProfileButton = document.getElementById('user-profile-exit');

    exitFromProfileButton.addEventListener('click', () => {
        fetch('/Identity/Logout')
            .then(() => {
                location.reload();
            })
    });
}
/*Выход из профиля пользователя*/


/*Загрузка каналов с группы (AJAX)*/

var logos = document.getElementsByClassName('logos-item');
var navigation_load = document.getElementsByClassName('navigation-group-load');

var previous_logo = logos[0];

for (var lox of logos) {
    lox.addEventListener('click', LoadNavigationPage.bind(null, lox));
}

logos[0].click();

function LoadNavigationPage(Group) {
    previous_logo.classList.remove('active');
    Group.classList.add('active');
    fetch("/Channel/GetChannels/" + Group.id)
        .then(response => {
            if (response.ok) {
                return response.text();
            } else {
                location.reload();
            }
        })
        .then(messages => {
            document.getElementById('navigation-group-load').innerHTML = messages;
            LoadEventsAfterGroupClick();
        })

    previous_logo = Group;
}
/*Загрузка каналов с группы (AJAX)*/


/*Загрузка всех действий после нажатия на группу*/
function LoadEventsAfterGroupClick() {
    LoadAddSettingsForm();
    CountChannelButton();
    AddChannelsListener();
}
/*Загрузка всех действий после нажатия на группу*/


/*Добавление группы*/
const add_group_form = document.getElementById('add-group-form');
const add_group_form_text = document.getElementById('add-group-text');
const add_group_button = document.getElementById('add-group-button');

add_group_button.addEventListener('click', () => {
    fetch("/Group/Form")
        .then(response => {
            if (response.ok) {
                return response.text();
            } else {
                return response.text().then(error => {
                    throw new Error(error);
                });
            }
        })
        .then(messages => {
            add_group_form_text.innerHTML = messages;
            add_group_form.style.display = 'flex';
        })
        .catch(error => {
            alert(error.message);
        })
    setTimeout(addGroupButtonDisappearance, 2);

})
/*Добавление группы*/


/*Форма для переименование каналла */
function RenameChannelForm(ChannelId, event) {
    event.stopPropagation();

    const renameChannel = document.getElementById('rename-channel-' + ChannelId);
    var parentRenameChannel = renameChannel.parentNode;

    var parentText = parentRenameChannel.innerHTML;


    fetch("/Channel/RenameForm")
        .then(response => {
            return response.text();
        })
        .then(text => {
            parentRenameChannel.innerHTML = text;
            RenameChannel();
        })


    document.addEventListener('click', RenameChannelFormClickOutside, { capture: true });
    function RenameChannelFormClickOutside(event) {

        const isClickInside = parentRenameChannel.contains(event.target);

        if (!isClickInside) {
            parentRenameChannel.innerHTML = parentText;
            document.removeEventListener('click', RenameChannelFormClickOutside, { capture: true });
        }
        else {

            if (event.target.closest('.channel-cancel') || event.target.closest('.channel-sumbit')) {
                return;
            }

            event.stopPropagation();
        }
    }

    function RenameChannel() {
        var channelSumbit = document.getElementById('channel-add-form-sumbit');
        var canselSumbit = document.getElementById('channel-add-form-cancel');

        canselSumbit.addEventListener('click', (event) => {
            event.stopPropagation();
            event.preventDefault();

            parentRenameChannel.innerHTML = parentText;
            document.removeEventListener('click', RenameChannelFormClickOutside, { capture: true });
        });

        channelSumbit.addEventListener('click', (event) => {
            event.stopPropagation();
            event.preventDefault();

            var renameForm = document.getElementById('channel-rename-form');

            const formData = new FormData(renameForm);
            formData.append("channelId", ChannelId);

            fetch("/Channel/RenameChannel", {
                method: 'POST',
                body: formData
            })
                .then(response => {
                    if (response.ok) {
                        return response.text();
                    } else {
                        return response.text().then(error => {
                            throw new Error(error);
                        });
                    }
                })
                .then(text => {
                    parentRenameChannel.textContent = '#' + formData.get('channelName');
                    parentRenameChannel.appendChild(renameChannel);
                    document.removeEventListener('click', RenameChannelFormClickOutside, { capture: true });
                    parentRenameChannel.click();
                })
                .catch(error => {
                    alert(error.message);
                })
        });

    }

}
/*Форма для переименование каналла */


/*Исчезание кнопки группы*/
function addGroupButtonDisappearance() {
    document.addEventListener('click', add_group_Click_Inside);
}

function add_group_Click_Inside(event) {
    const isClickInside = add_group_form_text.contains(event.target);

    if (!isClickInside) {
        add_group_form.style.display = 'none';
        add_group_form_text.innerHTML = "";
        document.removeEventListener('click', add_group_Click_Inside);
    }
}
/*Исчезание кнопки группы*/


/*Загрузка формы настройки группы*/
function LoadAddSettingsForm() {

    var add_channel_button = document.getElementById('group-settings');
    var dasdasd = document.getElementsByClassName('navigation-group-title-settings');
    var channel_settings_form = document.getElementById('group-settings-form');

    add_channel_button.addEventListener('click', () => {
        var group_active = document.getElementsByClassName('logos-item active')[0].id;

        fetch("/Group/SettingsForm/" + group_active)
            .then(response => {
                if (response.ok) {
                    return response.text();
                } else {
                    return response.text().then(error => {
                        throw new Error(error);
                    });
                }
            })
            .then(message => {
                channel_settings_form.innerHTML = message;
                setTimeout(SettingsCloseListener, 2);
                GroupSettingsLabel();
                RenameOrAddUser();
            })
            .catch(error => {
                alert(error.message);
            })

    });
}
/*Загрузка формы настройки группы*/


/*Закрытие настрек группы*/
function SettingsCloseListener() {
    document.addEventListener('click', SettingsClose);
}


function SettingsClose(event) {
    var channel_settings_form = document.getElementById('group-settings-form');

    const isClickInside = channel_settings_form.contains(event.target);

    if (!isClickInside) {
        channel_settings_form.innerHTML = "";
        document.removeEventListener('click', SettingsClose);
    }

}
/*Закрытие настрек группы*/


/*Создание новой группы*/
function CreateNewGroup(event) {
    event.preventDefault();

    var CreateGroupForm = document.getElementById('group-create-form');

    const CreateGroupFormData = new FormData(CreateGroupForm);

    fetch("/Group/Add", {
        method: 'POST',
        body: CreateGroupFormData
    })
        .then(response => {
            if (response.ok) {
                location.reload();
            }
            else {
                return response.text().then(error => {
                    throw new Error(error);
                });
            }
        })
        .catch(error => {
            alert(error.message);
        })

}
/*Создание новой группы*/


/*Вступление в группу*/
function JoinInGroup(event) {
    event.preventDefault();
    event.stopPropagation();

    var GroupCodeForm = document.getElementById('GroupCodeForm');

    const groupCodeFormData = new FormData(GroupCodeForm);

    fetch("/Group/Join", {
        method: 'POST',
        body: groupCodeFormData
    })
        .then(response => {
            if (response.ok) {
                location.reload();
            } else {
                return response.text().then(error => {
                    throw new Error(error);
                });
            }
        })
        .catch(error => {
            alert(error.message);
        })
}
/*Вступление в группу*/


/*Натройки текста для формы настроек группы*/
function GroupSettingsLabel() {

    const methodSelect = document.getElementById('method');
    const nameLabelSettings = document.getElementById('name-label-settings');
    const buttonLabelSettings = document.getElementById('rename-addUser-button');
    const textLabelSettings = document.getElementById('text-label-settings');

    methodSelect.addEventListener('change', () => {
        switch (methodSelect.value) {
            case 'rename':
                nameLabelSettings.textContent = "Новое название группы:";
                buttonLabelSettings.textContent = "Переименовать";
                textLabelSettings.type = "text";
                break;

            case 'addUser':
                nameLabelSettings.textContent = "Сколько пользователей смогут присоединиться:";
                buttonLabelSettings.textContent = "Пригласить";
                textLabelSettings.type = "text";
                break;

            case 'deleteGroup':
                nameLabelSettings.textContent = "Вы уверены?";
                buttonLabelSettings.textContent = "Удалить";
                textLabelSettings.type = "checkbox";
                break;

            case 'changeGroupAvatar':
                nameLabelSettings.textContent = "Выберите картинку";
                buttonLabelSettings.textContent = "Изменить";
                textLabelSettings.type = "file";
                textLabelSettings.addEventListener('change', ChangeGroupAvatar)
                break;
        };

    });

    function ChangeGroupAvatar(event) {
        if (textLabelSettings.type != "file") {
            textLabelSettings.removeEventListener('change', ChangeGroupAvatar);
            return;
        }

        var reader = new FileReader();

        fetch("/Group/ImageCutForm")
            .then(response => {
                return response.text();
            })
            .then(text => {
                var container = document.createElement("div");
                container.innerHTML = text;
                document.body.append(container);

                container = document.getElementById('croop-image');

                var croppie = new Croppie(container, {
                    viewport: { width: 200, height: 200, type: 'circle' },
                    boundary: { width: 300, height: 300 },
                });

                reader.onload = function (event) {
                    croppie.bind({
                        url: event.target.result
                    });
                };

                reader.readAsDataURL(event.target.files[0]);

                AddEventClosecontainer();

                document.getElementById('croop-image-confirm').addEventListener('click', () => {
                    cropAndSaveImage(croppie);
                });
            })


        function AddEventClosecontainer() {
            document.addEventListener('click', CloseContainer);
        }

        function CloseContainer(event) {
            var cropImageContainer = document.getElementById('croop-image-container');

            var isClickInContaine = cropImageContainer.contains(event.target);

            if (!isClickInContaine) {
                document.getElementById('image-container').remove();
                document.removeEventListener('click', CloseContainer);
            }
        }

        var croppedImage = null;

        function cropAndSaveImage(croppie) {
            croppie.result('base64').then(function (result) {
                croppedImage = result;
                saveImage();
            });
        }

        function saveImage() {
            if (croppedImage) {

                var formData = new FormData();
                formData.append("imageData", croppedImage);
                formData.append("idGroup", document.getElementsByClassName('logos-item active')[0].id);

                fetch("/Group/ChangeGroupAvatar", {
                    method: 'POST',
                    body: formData
                })
                    .then(response => {
                        return response.text();
                    })
                    .then(text => {
                        location.reload();
                    });
            }

        }

    }
}
/*Натройки текста для формы настроек группы*/


/*Смена имени группы или добавление пользователей*/
function RenameOrAddUser() {
    var add_channel_buttoon = document.getElementById('rename-addUser-button');

    add_channel_buttoon.addEventListener('click', (event) => {
        var id_group = document.getElementsByClassName('logos-item active')[0];
        var GorupForm = document.getElementById('RenameForm');

        event.preventDefault();

        const formData = new FormData(GorupForm);

        fetch("/Group/Settings/" + id_group.id, {
            method: 'POST',
            body: formData
        })
            .then(response => {
                return response.text().then(text => {
                    alert(text);
                    id_group.click();
                })
            })

    });

}
/*Смена имени группы или добавление пользователей*/


/*Кнопка добавления каналов*/
var AddChannelButtonPress = false;
function CountChannelButton() {

    var countChannelElement = document.getElementById('channels-count');
    var originCount = countChannelElement.textContent;

    countChannelElement.addEventListener('mouseover', () => {
        countChannelElement.textContent = "+";
    });

    countChannelElement.addEventListener('mouseout', () => {
        countChannelElement.textContent = originCount;
    });

    countChannelElement.addEventListener('click', (event) => {
        var id_group = document.getElementsByClassName('logos-item active')[0];

        var channels = document.getElementById('channels-block');
        var channelsHtml = channels.innerHTML;

        fetch("/Channel/AddChannelForm/" + id_group.id)
            .then(response => {
                if (response.ok) {
                    return response.text();
                } else {
                    return response.text().then(error => {
                        throw new Error(error);
                    });
                }
            })
            .then(text => {
                if (AddChannelButtonPress == false) {
                    channels.innerHTML = text;
                    channels.innerHTML += channelsHtml;
                    AddChannel(event);
                }
                AddChannelButtonPress = true;
            })
            .catch(error => {
                alert(error.message);
            })

    });
}
/*Кнопка добавления каналов*/


/*Форма добовления канала*/
function AddChannel(event) {
    const channelFormSumbit = document.getElementById('channel-add-form-sumbit');
    const channelFormCancel = document.getElementById('channel-add-form-cancel');

    channelFormSumbit.addEventListener('click', (event) => {
        var id_group = document.getElementsByClassName('logos-item active')[0];
        const channelForm = document.getElementById('channel-add-form');

        AddChannelButtonPress = false;

        event.preventDefault();
        event.stopPropagation();

        const formData = new FormData(channelForm);
        formData.append("groupId", id_group.id)

        fetch("/Channel/AddChannel", {
            method: 'POST',
            body: formData
        })
            .then(response => {
                if (response.ok) {
                    return response.text();
                } else {
                    return response.text().then(error => {
                        throw new Error(error);
                    });
                }
            })
            .then(text => {
                id_group.click();
            })
            .catch(error => {
                alert(error.message);
            })

    });

    channelFormCancel.addEventListener('click', (event) => {
        event.preventDefault();
        var id_group = document.getElementsByClassName('logos-item active')[0];
        id_group.click();
        AddChannelButtonPress = false;
    })

}
/*Форма добовления канала*/


/*Установка прослушки на каналы*/
var previewChannelId;
function AddChannelsListener() {
    var allChannels = document.getElementsByClassName('navigation-group-channels-name-item');

    for (var eblan of allChannels) {
        eblan.addEventListener('click', LoadMessages.bind(null, eblan));
        eblan.addEventListener('click', ConnetAndDisconect.bind(null, eblan.id));
    }

    if (allChannels[0] == null) {

    } else {
        allChannels[0].click();
    }
}
/*Установка прослушки на каналы*/


/*Загрузка сообщений из канала*/
function LoadMessages(channel) {

    var activeChannels = Array.from(document.getElementsByClassName('navigation-group-channels-name-item active'));

    for (var active of activeChannels) {
        active.classList.remove('active');
    }

    channel.classList.add('active');

    const chatForm = document.getElementById('chat-form');

    fetch("/Chat/Form/" + channel.id)
        .then(response => {
            return response.text();
        })
        .then(text => {
            chatForm.innerHTML = text;

            SendMessageEvent(channel.id);

            var chat = document.getElementById('chat-messages');
            chat.scrollTop = chat.scrollHeight;

        })

}
/*Загрузка сообщений из канала*/


/*Событие на отправку сообщений*/
function SendMessageEvent(channel) {
    const messageInput = document.getElementById('chat-sender-text-input');

    messageInput.addEventListener('keypress', function (e) {
        if (e.key == 'Enter') {
            SendMessage(channel);
        }
    })

}
/*Событие на отправку сообщений*/


/*Событие на отправку изображений*/
function SendImageEvent(ChannelId) {
    document.getElementById('chat-sender-image').click();
}
/*Событие на отправку изображений*/


/*Отправка изображения*/
function SendImage(channelId) {
    var fileInput = document.getElementById('chat-sender-image');
    const file = fileInput.files[0];

    const reader = new FileReader();
    reader.onload = function (e) {

        var formData = new FormData();
        formData.append('image', file);
        formData.append('channelId', channelId)

        fetch('/Message/Image', {
            method: 'POST',
            body: formData
        })
            .then(response => {
                return response.text();
            })
            .then(text => {
                hubConnection.invoke("SendImage", channelId, text);
            })
    };

    reader.readAsArrayBuffer(file);
}
/*Отправка изображения*/


/*Подключение и отключение*/
function ConnetAndDisconect(ChannelId) {

    setTimeout(localFunction, 200);
    function localFunction() {

        if (hubConnection.connectionState == signalR.HubConnectionState.Connected) {

            if (previewChannelId != null) {
                hubConnection.invoke("RemoveUserFromChannel", previewChannelId.toString());
            }

            previewChannelId = ChannelId;

            hubConnection.invoke("AddUserToChannel", ChannelId.toString());
        }
        else {
            console.log("Ne podkluchen");
            setTimeout(localFunction, 200);
        }
    }
}
/*Подключение и отключение*/


/*Отправка сообщений*/
function SendMessage(channelId) {
    const messageInput = document.getElementById('chat-sender-text-input');

    var messageText = messageInput.value;

    hubConnection.invoke("SendMessage", channelId.toString(), messageText);

    messageInput.value = '';
}
/*Отправка сообщений*/


/*Принятие сообщений*/
TakeMessage();

function TakeMessage() {
    hubConnection.on("Receive", function (message, id) {

        var chat = document.getElementById('chat-messages');
        chat.innerHTML += message;

        setTimeout(() => {
            chat.scrollTop = chat.scrollHeight;
        }, 100);
    });

    hubConnection.on("Send", function (message) {
        console.log(message);
    });
}
/*Принятие сообщений*/


/*Смена аватарки пользователя*/
function ClickOnInputAvatar(event) {
    document.getElementById('profile-avatar-input').click();
    event.stopPropagation();
}

function ChangeUserAvatar(event) {
    var reader = new FileReader();

    var container = null;

    fetch("/Group/ImageCutForm")
        .then(response => {
            return response.text();
        })
        .then(text => {
            var container = document.createElement("div");
            container.innerHTML = text;
            document.body.append(container);

            container = document.getElementById('croop-image');

            var croppie = new Croppie(container, {
                viewport: { width: 200, height: 200, type: 'circle' },
                boundary: { width: 300, height: 300 },
            });

            reader.onload = function (event) {
                croppie.bind({
                    url: event.target.result
                });
            };

            reader.readAsDataURL(event.target.files[0]);

            AddEventClosecontainer();

            document.getElementById('croop-image-confirm').addEventListener('click', () => {
                cropAndSaveImage(croppie);
            });
        })

    function AddEventClosecontainer() {
        document.addEventListener('click', CloseContainer);
    }

    function CloseContainer(event) {
        var cropImageContainer = document.getElementById('croop-image-container');

        var isClickInContaine = cropImageContainer.contains(event.target);

        if (!isClickInContaine) {
            document.getElementById('image-container').remove();
            document.removeEventListener('click', CloseContainer);
        }
    }

    var croppedImage = null;

    function cropAndSaveImage(croppie) {
        croppie.result('base64').then(function (result) {
            croppedImage = result;
            saveImage();
        });
    }

    function saveImage() {
        if (croppedImage) {

            var formData = new FormData();
            formData.append("imageData", croppedImage);

            fetch("User/ChangeUserAvatar", {
                method: 'POST',
                body: formData
            })
                .then(response => {
                    return response.text();
                })
                .then(text => {
                    location.reload();
                });
        }
    }
}
/*Смена аватарки пользователя*/


/*Поиск сообщений*/
function ClickOnInputSearch(event) {
    document.getElementById('search-input').focus();
}


function MessagesTextSearch(event) {
    if (event.keyCode != 13) {
        return;
    }

    var inputText = document.getElementById('search-input');

    if (inputText.value.trim() == "") {
        inputText.value = "";
        return;
    }

    var targetText = inputText.value.trim().toLowerCase();

    inputText.value = "";

    var allElements = document.querySelectorAll('.user-message-text');
    var messagesText = Array.from(allElements).map(function (element) {
        return element.textContent.trim();
    });

    var similarity = stringSimilarity.findBestMatch(targetText, messagesText);

    console.log(similarity.bestMatch.target);

    if (similarity.bestMatch.target) {
        var index = similarity.bestMatchIndex;
        var chatMessages = document.getElementById('chat-messages');
        var targetElement = allElements[index];

        var messagePosition = targetElement.offsetTop - 150;

        chatMessages.scrollTo({
            top: messagePosition,
            behavior: 'smooth'
        });

        var userMessageNameData = targetElement.parentNode;
        var userMessage = userMessageNameData.parentNode;

        userMessage.style.backgroundColor = '#dbdbdb';

        setTimeout(() => {
            userMessage.style.backgroundColor = '';
        }, 2000);
    }
}
/*Поиск сообщений*/