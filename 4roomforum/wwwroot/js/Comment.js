(() => {
    "use strict";

    const page = document.querySelector(".reply-page");
    if (!page) return;

    const postId = page.dataset.postId;
    const isLoggedIn = page.dataset.login === "true";
    const comments = document.getElementById("commentsContainer");
    const status = document.getElementById("connectionStatus");
    let pendingDeleteId = null;

    const escapeHtml = (value) => {
        const element = document.createElement("div");
        element.textContent = value ?? "";
        return element.innerHTML;
    };

    const formatDate = (value) => new Date(value).toLocaleString();

    const setConnectionStatus = (text, online) => {
        status.textContent = text;
        status.classList.toggle("connection-online", online);
        status.classList.toggle("connection-offline", !online);
    };

    const formToJson = (form) => Object.fromEntries(new FormData(form).entries());

    const openComposer = (composer, context) => {
        if (!isLoggedIn) {
            window.location.href = "/Login/SignIn";
            return;
        }
        composer.hidden = false;
        const contextLabel = composer.querySelector("#composerContext");
        if (contextLabel) contextLabel.textContent = context;
        composer.querySelector("textarea")?.focus();
    };

    document.getElementById("openPostComposer")?.addEventListener("click", () => {
        openComposer(document.getElementById("postComposer"), "Comment on this post");
    });

    document.addEventListener("click", (event) => {
        const closeButton = event.target.closest("[data-close-composer]");
        if (closeButton) closeButton.closest(".composer-panel").hidden = true;

        const replyButton = event.target.closest(".comment-for-reply");
        if (replyButton) {
            if (!isLoggedIn) {
                window.location.href = "/Login/SignIn";
                return;
            }
            const form = replyButton.closest(".comment-card").querySelector(".nested-reply-form");
            form.hidden = false;
            form.querySelector("textarea")?.focus();
        }

        const cancelButton = event.target.closest(".cancel-nested-reply");
        if (cancelButton) cancelButton.closest(".nested-reply-form").hidden = true;

        const deleteButton = event.target.closest(".delete-button");
        if (deleteButton) {
            pendingDeleteId = deleteButton.dataset.replyId;
            bootstrap.Modal.getOrCreateInstance(document.getElementById("confirmDeleteModal")).show();
        }

        const editButton = event.target.closest(".edit-button");
        if (editButton) editReply(editButton.dataset.id);
    });

    document.addEventListener("submit", async (event) => {
        const form = event.target.closest(".reply-form");
        if (!form) return;
        event.preventDefault();

        const textarea = form.querySelector("textarea");
        if (!textarea.value.trim()) {
            form.classList.add("was-validated");
            textarea.focus();
            return;
        }

        const submitButton = form.querySelector("[type=submit]");
        submitButton.disabled = true;
        try {
            const response = await fetch("/Reply/Create", {
                method: "POST",
                body: new FormData(form),
                headers: { "X-Requested-With": "XMLHttpRequest" }
            });
            if (!response.ok) throw new Error("Unable to post the comment.");
            form.reset();
            form.classList.remove("was-validated");
            if (form.classList.contains("nested-reply-form")) form.hidden = true;
            else document.getElementById("postComposer").hidden = true;
        } catch (error) {
            console.error(error);
            alert("Unable to post your comment. Please try again.");
        } finally {
            submitButton.disabled = false;
        }
    });

    document.getElementById("confirmDeleteButton")?.addEventListener("click", async () => {
        if (!pendingDeleteId) return;
        try {
            const response = await fetch(`/Reply/Delete?id=${encodeURIComponent(pendingDeleteId)}`, {
                method: "POST",
                headers: { "X-Requested-With": "XMLHttpRequest" }
            });
            if (!response.ok) throw new Error("Unable to delete the comment.");
            bootstrap.Modal.getOrCreateInstance(document.getElementById("confirmDeleteModal")).hide();
        } catch (error) {
            console.error(error);
            alert("Unable to delete this comment. Please try again.");
        }
    });

    async function editReply(replyId) {
        const item = document.getElementById(`reply-${replyId}`);
        const content = item?.querySelector(".reply-content");
        if (!content) return;
        const original = content.textContent;
        const editor = document.createElement("div");
        editor.className = "edit-reply-form";
        editor.innerHTML = `<textarea class="form-control mb-2">${escapeHtml(original)}</textarea>
            <button class="btn btn-sm btn-primary save-edit">Save</button>
            <button class="btn btn-sm btn-secondary cancel-edit">Cancel</button>`;
        content.replaceWith(editor);

        editor.querySelector(".cancel-edit").addEventListener("click", () => {
            editor.replaceWith(createContentElement(original));
        });
        editor.querySelector(".save-edit").addEventListener("click", async () => {
            const updated = editor.querySelector("textarea").value.trim();
            if (!updated) return;
            const response = await fetch(`/Reply/Update/${replyId}`, {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ replyContent: updated, replyDate: new Date().toISOString(), isEdited: true })
            });
            if (!response.ok) {
                alert("Unable to update this comment.");
                return;
            }
            editor.replaceWith(createContentElement(updated));
        });
    }

    const createContentElement = (content) => {
        const element = document.createElement("p");
        element.className = "reply-content mt-3 mb-0";
        element.textContent = content;
        return element;
    };

    document.addEventListener("click", async (event) => {
        const button = event.target.closest(".like-btn, .dislike-btn");
        if (!button) return;
        if (!isLoggedIn) {
            window.location.href = "/Login/SignIn";
            return;
        }
        const replyId = button.dataset.replyId;
        const item = document.getElementById(`reply-${replyId}`);
        const icon = button.querySelector("i");
        const isLike = button.classList.contains("like-btn");
        const vote = icon.classList.contains("fa-solid") ? 0 : (isLike ? 1 : -1);
        const userId = document.querySelector(".reply-form input[name=RepliedBy]")?.value;
        const response = await fetch(`/Reply/React?replyId=${replyId}&userId=${userId}&vote=${vote}`, { method: "POST" });
        if (!response.ok) return;
        const result = await response.json();
        if (!result.success) return;
        item.querySelector(".like-count").textContent = result.like;
        item.querySelector(".dislike-count").textContent = result.dis_like;
        item.querySelector(".like-btn i").className = `fa-${result.react_type === 1 ? "solid" : "regular"} fa-thumbs-up`;
        item.querySelector(".dislike-btn i").className = `fa-${result.react_type === -1 ? "solid" : "regular"} fa-thumbs-down`;
    });

    const connection = new signalR.HubConnectionBuilder()
        .withUrl(`/comment?postId=${encodeURIComponent(postId)}`)
        .withAutomaticReconnect()
        .build();

    connection.on("ReceiveComment", (comment) => {
        if (String(comment.postId) !== String(postId) || document.getElementById(`reply-${comment.replyId}`)) return;
        document.getElementById("emptyComments")?.remove();
        comments.insertAdjacentHTML("afterbegin", renderComment(comment));
    });
    connection.on("ReplyDeleted", (replyId) => document.getElementById(`reply-${replyId}`)?.remove());
    connection.on("ReplyUpdated", (reply) => {
        const content = document.querySelector(`#reply-${reply.replyId} .reply-content`);
        if (content) content.replaceWith(createContentElement(reply.replyContent));
    });
    connection.onreconnecting(() => setConnectionStatus("Reconnecting...", false));
    connection.onreconnected(() => setConnectionStatus("Live updates", true));
    connection.onclose(() => setConnectionStatus("Offline", false));
    connection.start()
        .then(() => setConnectionStatus("Live updates", true))
        .catch((error) => {
            console.error(error);
            setConnectionStatus("Offline", false);
        });

    function renderComment(comment) {
        const quote = comment.replyToReply
            ? `<blockquote class="quoted-reply p-2 bg-light border rounded"><strong class="text-primary">${escapeHtml(comment.replyToUserName)} said:</strong><br>${escapeHtml(comment.replyToContent)}</blockquote>`
            : "";
        return `<article class="comment-item row mb-4" id="reply-${comment.replyId}" data-reply-id="${comment.replyId}">
            <div class="col-md-2 text-center"><div class="avatar"><img src="/imgs/${escapeHtml(comment.avatar || "default-avatar.png")}" alt="${escapeHtml(comment.userName)} avatar"></div><p class="mt-2 fw-bold mb-0">${escapeHtml(comment.userName)}</p></div>
            <div class="col-md-10"><div class="card comment-card"><div class="card-header bg-light"><strong class="post-time">Posted at ${formatDate(comment.replyDate)}</strong></div>
            <div class="card-body">${quote}<p class="reply-content mt-3 mb-0">${escapeHtml(comment.replyContent)}</p></div>
            <div class="card-footer"><div class="d-flex align-items-center gap-2">
                <button class="reaction-btn like-btn" data-reply-id="${comment.replyId}" type="button"><i class="fa-regular fa-thumbs-up"></i> <span class="like-count">0</span></button>
                <button class="reaction-btn dislike-btn" data-reply-id="${comment.replyId}" type="button"><i class="fa-regular fa-thumbs-down"></i> <span class="dislike-count">0</span></button>
                <button class="reaction-btn comment-for-reply" data-reply-id="${comment.replyId}" type="button"><i class="fa-solid fa-comment"></i> Reply</button>
            </div><form class="reply-form nested-reply-form mt-3" data-post-id="${postId}" hidden>
                <input type="hidden" name="PostId" value="${postId}"><input type="hidden" name="RepliedBy" value="${document.querySelector("[name=RepliedBy]")?.value || ""}"><input type="hidden" name="ReplyToReply" value="${comment.replyId}">
                <textarea class="form-control" name="ReplyContent" rows="2" maxlength="2000" placeholder="Reply..." required></textarea>
                <div class="text-end mt-2"><button class="btn btn-sm btn-outline-secondary cancel-nested-reply" type="button">Cancel</button> <button class="btn btn-sm btn-primary" type="submit">Post reply</button></div>
            </form></div></div></div></article>`;
    }

    document.getElementById("shareButton")?.addEventListener("click", () => {
        const url = encodeURIComponent(window.location.href);
        window.open(`https://www.facebook.com/sharer/sharer.php?u=${url}`, "facebook-share-dialog", "width=800,height=600");
    });
})();

async function toggleLike(element) {
    try {
        const result = await (await fetch(`/Post/LikeOrUnlikePost?postId=${element.closest(".reply-page").dataset.postId}`, { method: "POST" })).json();
        if (!result.success) throw new Error("Unable to update post reaction.");
        document.getElementById("likeCount").textContent = result.totalLikes;
        element.classList.toggle("liked", result.isLiked);
    } catch (error) {
        console.error(error);
        window.location.href = "/Login/SignIn";
    }
}
