-> main

=== main ===
-> npc1_conversation

=== npc1_conversation ===
Xin chào anh hùng!
Cần nâng cấp trang bị?
* [Có chứ!] Tất nhiên rồi.
    # open_panel_npc1
    -> END
* [Không, cảm ơn.] Tạm biệt.
    -> END

=== npc2_conversation ===
Xin chào chiến binh!
Bạn có muốn nâng cấp sức mạnh?
* [Có chứ!] Tôi sẽ phù phép cho bạn
    # open_panel_npc2
    -> END
* [Không, cảm ơn.] Tạm biệt.
    -> END
