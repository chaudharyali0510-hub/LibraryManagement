(function () {
    'use strict';

    const STORAGE_KEYS = {
        SESSION: 'chatbot_session_id',
        HISTORY: 'chatbot_history'
    };

    const WELCOME_MESSAGE = [
        '👋 Welcome!',
        '',
        "I'm your AI Library Assistant.",
        '',
        'I can help you:',
        '📚 Search books',
        '👤 Find authors',
        '🏢 Find publishers',
        '📖 Check availability',
        '⭐ Recommend books',
        '',
        'How can I help you today?'
    ].join('\n');

    let isSending = false;

    function getContainer() {
        return document.getElementById('chatbot-container');
    }

    function getWebhookUrl() {
        var container = getContainer();
        return container ? container.dataset.webhookUrl : '';
    }

    function getMessagesEl() {
        return document.getElementById('chatbot-messages');
    }

    function getInputEl() {
        return document.getElementById('chatbot-input');
    }

    function getSendBtn() {
        return document.getElementById('chatbot-send');
    }

    function getWindowEl() {
        return document.getElementById('chatbot-window');
    }

    function getBtnEl() {
        return document.getElementById('chatbot-btn');
    }

    // --- Session ---
    function generateSessionId() {
        try {
            return crypto.randomUUID();
        } catch (e) {
            return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
                var r = Math.random() * 16 | 0;
                return (c === 'x' ? r : (r & 0x3 | 0x8)).toString(16);
            });
        }
    }

    function getSessionId() {
        var id = localStorage.getItem(STORAGE_KEYS.SESSION);
        if (!id) {
            id = generateSessionId();
            localStorage.setItem(STORAGE_KEYS.SESSION, id);
        }
        return id;
    }

    // --- Chat History ---
    function saveChatHistory(messages) {
        try {
            localStorage.setItem(STORAGE_KEYS.HISTORY, JSON.stringify(messages));
        } catch (e) {
            // localStorage full or unavailable
        }
    }

    function loadChatHistory() {
        try {
            var data = localStorage.getItem(STORAGE_KEYS.HISTORY);
            return data ? JSON.parse(data) : [];
        } catch (e) {
            return [];
        }
    }

    // --- Scroll ---
    function scrollToBottom() {
        var el = getMessagesEl();
        if (el) {
            el.scrollTop = el.scrollHeight;
        }
    }

    // --- Timestamp ---
    function getTimestamp() {
        var now = new Date();
        var h = now.getHours().toString().padStart(2, '0');
        var m = now.getMinutes().toString().padStart(2, '0');
        return h + ':' + m;
    }

    // --- Markdown Rendering ---
    function renderMarkdown(text) {
        var html = escapeHtml(text);
        html = html.replace(/\*\*(.+?)\*\*/g, '<strong>$1</strong>');
        var lines = html.split('\n');
        var result = [];
        var inList = false;

        for (var i = 0; i < lines.length; i++) {
            var line = lines[i];
            var listMatch = line.match(/^[\s]*[-*]\s+(.*)/);
            if (listMatch) {
                if (!inList) {
                    result.push('<ul>');
                    inList = true;
                }
                result.push('<li>' + listMatch[1] + '</li>');
            } else {
                if (inList) {
                    result.push('</ul>');
                    inList = false;
                }
                if (line === '') {
                    result.push('<br>');
                } else {
                    result.push(line);
                }
            }
        }
        if (inList) {
            result.push('</ul>');
        }

        return result.join('\n');
    }

    function escapeHtml(text) {
        var div = document.createElement('div');
        div.appendChild(document.createTextNode(text));
        return div.innerHTML;
    }

    // --- Append Messages ---
    function appendUserMessage(text) {
        var el = getMessagesEl();
        if (!el) return;

        var wrapper = document.createElement('div');
        wrapper.className = 'message message-user';

        var content = document.createElement('div');
        content.className = 'message-content';
        content.textContent = text;
        wrapper.appendChild(content);

        var time = document.createElement('span');
        time.className = 'message-time';
        time.textContent = getTimestamp();
        wrapper.appendChild(time);

        el.appendChild(wrapper);
        scrollToBottom();
    }

    function appendBotMessage(text) {
        var el = getMessagesEl();
        if (!el) return;

        var wrapper = document.createElement('div');
        wrapper.className = 'message message-bot';

        var content = document.createElement('div');
        content.className = 'message-content';
        content.innerHTML = renderMarkdown(text);
        wrapper.appendChild(content);

        var time = document.createElement('span');
        time.className = 'message-time';
        time.textContent = getTimestamp();
        wrapper.appendChild(time);

        var actions = document.createElement('div');
        actions.className = 'message-actions';

        var copyBtn = document.createElement('button');
        copyBtn.className = 'message-copy-btn';
        copyBtn.innerHTML = '<i class="fas fa-copy"></i> Copy';
        copyBtn.onclick = function () { copyMessage(text, copyBtn); };
        actions.appendChild(copyBtn);

        wrapper.appendChild(actions);

        el.appendChild(wrapper);
        scrollToBottom();
    }

    function appendErrorMessage(text) {
        var el = getMessagesEl();
        if (!el) return;

        var wrapper = document.createElement('div');
        wrapper.className = 'message message-error';

        var content = document.createElement('div');
        content.className = 'message-content';
        content.textContent = text;
        wrapper.appendChild(content);

        el.appendChild(wrapper);
        scrollToBottom();
    }

    function showWelcome() {
        appendBotMessage(WELCOME_MESSAGE);
    }

    // --- Typing Indicator ---
    function showTyping() {
        var el = getMessagesEl();
        if (!el) return;

        var indicator = document.createElement('div');
        indicator.className = 'typing-indicator';
        indicator.id = 'chatbot-typing';

        for (var i = 0; i < 3; i++) {
            var dot = document.createElement('span');
            dot.className = 'typing-dot';
            indicator.appendChild(dot);
        }

        el.appendChild(indicator);
        scrollToBottom();
    }

    function hideTyping() {
        var indicator = document.getElementById('chatbot-typing');
        if (indicator) {
            indicator.remove();
        }
    }

    // --- Copy ---
    function copyMessage(text, btn) {
        if (navigator.clipboard) {
            navigator.clipboard.writeText(text).then(function () {
                showCopyFeedback(btn);
            });
        } else {
            var textarea = document.createElement('textarea');
            textarea.value = text;
            textarea.style.position = 'fixed';
            textarea.style.opacity = '0';
            document.body.appendChild(textarea);
            textarea.select();
            document.execCommand('copy');
            document.body.removeChild(textarea);
            showCopyFeedback(btn);
        }
    }

    function showCopyFeedback(btn) {
        var original = btn.innerHTML;
        btn.innerHTML = '<i class="fas fa-check"></i> Copied';
        setTimeout(function () {
            btn.innerHTML = original;
        }, 1500);
    }

    // --- Send ---
    function sendMessage() {
        if (isSending) return;

        var input = getInputEl();
        var text = input ? input.value.trim() : '';

        if (!text) return;
        if (text.length > 500) return;

        isSending = true;
        var sendBtn = getSendBtn();
        if (sendBtn) sendBtn.disabled = true;

        input.value = '';
        if (sendBtn) sendBtn.disabled = true;
        updateCharCount();

        appendUserMessage(text);

        var history = loadChatHistory();
        history.push({ role: 'user', text: text, time: getTimestamp() });
        saveChatHistory(history);

        showTyping();

        var url = getWebhookUrl();
        if (!url) {
            hideTyping();
            var errMsg = 'Sorry, the AI assistant is currently unavailable. Please try again later.';
            appendErrorMessage(errMsg);
            isSending = false;
            if (sendBtn) sendBtn.disabled = false;
            focusInput();
            return;
        }

        var controller = new AbortController();
        var timeoutId = setTimeout(function () { controller.abort(); }, 30000);

        fetch(url, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({
                sessionId: getSessionId(),
                userName: 'Guest',
                message: text
            }),
            signal: controller.signal
        })
        .then(function (response) {
            clearTimeout(timeoutId);
            if (!response.ok) throw new Error('HTTP ' + response.status);
            return response.json();
        })
        .then(function (data) {
            clearTimeout(timeoutId);
            hideTyping();
            console.log('n8n response:', data);
            var responseData = Array.isArray(data) ? data[0] : data;
            var reply = responseData && (responseData.reply || responseData.output || responseData.response || responseData.text || responseData.message);
            if (reply) {
                appendBotMessage(reply);
                history = loadChatHistory();
                history.push({ role: 'assistant', text: reply, time: getTimestamp() });
                saveChatHistory(history);
            } else {
                appendErrorMessage('Sorry, the AI assistant is currently unavailable. Please try again later.');
            }
        })
        .catch(function () {
            clearTimeout(timeoutId);
            hideTyping();
            appendErrorMessage('Sorry, the AI assistant is currently unavailable. Please try again later.');
        })
        .finally(function () {
            isSending = false;
            if (sendBtn) sendBtn.disabled = false;
            focusInput();
        });
    }

    // --- Clear Chat ---
    function clearChat() {
        if (!confirm('Clear all chat messages?')) return;

        var el = getMessagesEl();
        if (el) el.innerHTML = '';
        localStorage.removeItem(STORAGE_KEYS.HISTORY);
        showWelcome();
        var history = [{ role: 'assistant', text: WELCOME_MESSAGE, time: getTimestamp() }];
        saveChatHistory(history);
        focusInput();
    }

    // --- Open / Close ---
    function openChat() {
        var window = getWindowEl();
        var btn = getBtnEl();
        if (!window) return;

        window.classList.add('open');
        if (btn) btn.classList.add('hidden');

        var history = loadChatHistory();
        var el = getMessagesEl();
        if (el) el.innerHTML = '';

        if (history.length === 0) {
            showWelcome();
            history = [{ role: 'assistant', text: WELCOME_MESSAGE, time: getTimestamp() }];
            saveChatHistory(history);
        } else {
            for (var i = 0; i < history.length; i++) {
                var msg = history[i];
                if (msg.role === 'user') {
                    appendUserMessage(msg.text);
                } else if (msg.role === 'assistant') {
                    appendBotMessage(msg.text);
                }
            }
        }

        scrollToBottom();
        focusInput();
    }

    function closeChat() {
        var window = getWindowEl();
        var btn = getBtnEl();
        if (!window) return;

        window.classList.remove('open');
        if (btn) btn.classList.remove('hidden');
    }

    // --- Input helpers ---
    function focusInput() {
        var input = getInputEl();
        if (input && getWindowEl() && getWindowEl().classList.contains('open')) {
            setTimeout(function () { input.focus(); }, 100);
        }
    }

    function updateCharCount() {
        var input = getInputEl();
        var counter = document.getElementById('chatbot-charcount');
        if (input && counter) {
            var len = input.value.length;
            counter.textContent = len + '/500';
            counter.style.color = len >= 450 ? '#ef4444' : '#94a3b8';
        }
    }

    // --- Init ---
    function init() {
        var container = getContainer();
        if (!container) return;

        // Input event
        var input = getInputEl();
        if (input) {
            input.addEventListener('keydown', function (e) {
                if (e.key === 'Enter' && !e.shiftKey) {
                    e.preventDefault();
                    sendMessage();
                }
            });
            input.addEventListener('input', updateCharCount);
        }

        var sendBtn = getSendBtn();
        if (sendBtn) {
            // Enable/disable send based on input
            input.addEventListener('input', function () {
                var text = input.value.trim();
                sendBtn.disabled = text.length === 0 || text.length > 500 || isSending;
            });
        }

        // Listen for Escape to close
        document.addEventListener('keydown', function (e) {
            if (e.key === 'Escape') {
                var window = getWindowEl();
                if (window && window.classList.contains('open')) {
                    closeChat();
                }
            }
        });

        updateCharCount();
    }

    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', init);
    } else {
        init();
    }

    // Export functions globally for onclick handlers
    window.openChat = openChat;
    window.closeChat = closeChat;
    window.sendMessage = sendMessage;
    window.clearChat = clearChat;
    window.copyMessage = copyMessage;
})();
